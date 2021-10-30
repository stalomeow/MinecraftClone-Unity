require "block"

local playerModification = CS.Minecraft.ModificationSource.PlayerAction
local quaternionIdentity = CS.UnityEngine.Quaternion.identity
local fluidState = CS.Minecraft.PhysicSystem.PhysicState.Fluid
local collisionBelowFlag = CS.UnityEngine.CollisionFlags.Below
local gravity = create_block_behaviour()

function gravity:init(world, block)
    gravity.base.init(self, world, block)

    self.mass = 1
    self.gravity_multiplier = 1
    self.air_block_data = world.BlockDataTable:GetBlock("air")
end

function gravity:tick(x, y, z)
    local accessor = self.world.RWAccessor
    local block = accessor:GetBlock(x, y - 1, z)

    if block and block.PhysicState == fluidState then
        self.world.EntityManager:CreateBlockEntityAt(x, y, z, self:get_block_data())
        accessor:SetBlock(x, y, z, self.air_block_data, quaternionIdentity, playerModification)
    end
end

function gravity:entity_init(entity, context)
    entity.Mass = self.mass
    entity.GravityMultiplier = self.gravity_multiplier
end

function gravity:entity_on_collisions(entity, flags, context)
    if (flags & collisionBelowFlag) == collisionBelowFlag then
        local pos = entity.Position
        self.world.RWAccessor:SetBlock(pos.x, pos.y, pos.z, self:get_block_data(), quaternionIdentity, playerModification)
        self.world.EntityManager:DestroyEntity(entity)
    end
end

return gravity