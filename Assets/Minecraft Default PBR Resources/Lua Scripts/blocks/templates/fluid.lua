require "block"

local Vector3Int = CS.UnityEngine.Vector3Int
local playerModification = CS.Minecraft.ModificationSource.PlayerAction
local quaternionIdentity = CS.UnityEngine.Quaternion.identity
local fluid = create_block_behaviour()

function fluid:init(world, block)
    fluid.base.init(self, world, block)

    self.air_name = "air"
    self.max_level = 7
    self.levels = CS.System.Collections.Generic.Dictionary(Vector3Int, CS.System.Int32)()
end

function fluid:place(x, y, z)
    self.levels[Vector3Int(x, y, z)] = self.max_level
end

function fluid:destroy(x, y, z)
    self.levels:Remove(Vector3Int(x, y, z))
end

function fluid:set_fluid(x, y, z, rwAccessor, level)
    local block = rwAccessor:GetBlock(x, y, z)

    if not block then
        return
    end

    if block.InternalName == self.air_name then
        rwAccessor:SetBlock(x, y, z, self:get_block_data(), quaternionIdentity, playerModification)
        self.levels[Vector3Int(x, y, z)] = level

    elseif block.InternalName == self.InternalName then
        local key = Vector3Int(x, y, z)
        local exists, l = self.levels:TryGetValue(key)

        if not exists then
            self.levels[key] = self.max_level
        elseif l < level then
            self.levels[key] = level
            self.world:TickBlock(x, y, z)
        end
    end
end

function fluid:tick(x, y, z)
    local key = Vector3Int(x, y, z)
    local exists, level = self.levels:TryGetValue(key)

    if not exists then
        self.levels[key] = self.max_level
        level = self.max_level
    end

    local rwAccessor = self.world.RWAccessor
    self:set_fluid(x, y - 1, z, rwAccessor, level) -- 可以无限向下流

    level = level - 1

    if level == 0 then
        return
    end

    self:set_fluid(x - 1, y, z, rwAccessor, level)
    self:set_fluid(x + 1, y, z, rwAccessor, level)
    self:set_fluid(x, y, z - 1, rwAccessor, level)
    self:set_fluid(x, y, z + 1, rwAccessor, level)
end

return fluid