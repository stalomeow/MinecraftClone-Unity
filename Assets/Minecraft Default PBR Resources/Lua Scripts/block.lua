local behaviour = {} -- 默认行为

function behaviour:init(world, block)
    self.world = world
    self.__block = block
    print("init block behaviour: " .. block.InternalName)
end

function behaviour:tick(x, y, z)
    -- default implement
end

function behaviour:place(x, y, z)
    -- default implement
end

function behaviour:destroy(x, y, z)
    -- default implement
end

function behaviour:click(x, y, z)
    -- default implement
end

function behaviour:entity_init(entity, context)
    -- default implement
end

function behaviour:entity_destroy(entity, context)
    -- default implement
end

function behaviour:entity_update(entity, context)
    -- default implement
end

function behaviour:entity_fixed_update(entity, context)
    -- default implement
end

function behaviour:entity_on_collisions(entity, flags, context)
    -- default implement
end

function behaviour:get_block_data()
    return self.__block
end

--- 创建一个方块行为对象。
---
--- 如果不传入 `base` 参数，返回的对象将具有默认行为。
---
--- 如果传入 `base` 参数，返回的对象将会继承 `base` 的所有行为。
---
--- @param base? table
--- @return table
function create_block_behaviour(base)
    return setmetatable({
        base = base or behaviour
    }, {
        __index = function(table, key)
            local block = rawget(table, "__block")
            return block and block[key] or rawget(table, "base")[key]
        end
    })
end
