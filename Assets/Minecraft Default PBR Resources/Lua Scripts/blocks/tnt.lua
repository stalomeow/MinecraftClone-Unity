require "block"
local util = require "xlua.util"

tnt = create_block_behaviour()
local unityTime = CS.UnityEngine.Time
local unityColor = CS.UnityEngine.Color
local playerModification = CS.Minecraft.ModificationSource.PlayerAction
local quaternionIdentity = CS.UnityEngine.Quaternion.identity
local ignoreExplosionsFlag = CS.Minecraft.Configurations.BlockFlags.IgnoreExplosions
local assetManager = CS.Minecraft.Assets.AssetManager.Instance
local explosionEffectAssetName = "Assets/Minecraft Default PBR Resources/Effects/Explosion Effect.prefab"
local waitTime = 3
local explodeRadius = 5

function tnt:init(world, block)
    tnt.base.init(self, world, block)

    self.water_name = "water"
    self.lava_name = "lava"
    self.mass = 1
    self.gravity_multiplier = 1
    self.air_block_data = world.BlockDataTable:GetBlock("air")
end

function tnt:is_lava(x, y, z, accessor)
    local block = accessor:GetBlock(x, y, z)
    return block and block.InternalName == self.lava_name
end

function tnt:place(x, y, z)
    -- 边上有岩浆直接炸
    local accessor = self.world.RWAccessor
    local flag = self:is_lava(x - 1, y, z, accessor)
        or self:is_lava(x + 1, y, z, accessor)
        or self:is_lava(x, y - 1, z, accessor)
        or self:is_lava(x, y + 1, z, accessor)
        or self:is_lava(x, y, z - 1, accessor)
        or self:is_lava(x, y, z + 1, accessor)

    if flag then
        self:click(x, y, z)
    end
end

function tnt:click(x, y, z)
    self.world.EntityManager:CreateBlockEntityAt(x, y, z, self:get_block_data())
    self.world.RWAccessor:SetBlock(x, y, z, self.air_block_data, quaternionIdentity, playerModification)
end

function tnt:explode(center_x, center_y, center_z, radius, accessor)
    local sqrRadius = radius * radius

    for x = -radius, radius do for z = -radius, radius do for y = -radius, radius do
        if x * x + z * z + y * y <= sqrRadius then
            local world_x = center_x + x
            local world_y = center_y + y
            local world_z = center_z + z
            local block = accessor:GetBlock(world_x, world_y, world_z)

            if block then
                if block.InternalName == self.InternalName then
                    self:click(world_x, world_y, world_z) -- 炸到 TNT 的话，让这个 TNT 也炸
                elseif not block:HasFlag(ignoreExplosionsFlag) then
                    accessor:SetBlock(world_x, world_y, world_z, self.air_block_data, quaternionIdentity, playerModification)
                end
            end
        end
    end end end
end

function tnt:entity_init(entity, context)
    entity.Mass = self.mass
    entity.GravityMultiplier = self.gravity_multiplier

    -- 提前加载
    context.explosionEffectAsset = assetManager:LoadAsset(explosionEffectAssetName, typeof(CS.UnityEngine.GameObject))
end

function tnt:entity_on_collisions(entity, flags, context)
    if context.started then
        return
    end

    context.started = true
    entity:StartCoroutine(util.cs_generator(function()
        local time = 0
        local effectAsset = context.explosionEffectAsset

        while time < waitTime or not effectAsset.IsDone do
            time = time + unityTime.deltaTime
            local t = (math.cos(time * math.pi * 1.5) + 1) * 0.5
            local color = unityColor.Lerp(unityColor.grey, unityColor.white, t)
            entity.MaterialProperty:SetColor("_MainColor", color)
            coroutine.yield(nil)
        end

        local pos = entity.Position
        local accessor = entity.World.RWAccessor
        local block = accessor:GetBlock(pos.x, pos.y, pos.z)

        -- 水里爆炸不会破坏方块
        if block and block.InternalName ~= self.water_name then
            self:explode(pos.x, pos.y, pos.z, explodeRadius, accessor)
        end

        local effect = CS.UnityEngine.Object.Instantiate(context.explosionEffectAsset.Asset)
        effect.transform.position = pos

        local particle = CS.Minecraft.Lua.LuaUtility.GetParticleSystem(effect)
        particle:Play()

        entity.EnableRendering = false
        coroutine.yield(nil)

        -- 这里不删除资源了，后面可能还要用
        -- assetManager:UnloadAsset(context.explosionEffectAsset)
        self.world.EntityManager:DestroyEntity(entity)
    end))
end