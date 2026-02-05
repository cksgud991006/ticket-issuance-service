-- KEYS[1]: flight:booking:{flight_id}
-- ARGV[1]: seat_id
-- ARGV[2]: user_id

local status = redis.call('HGET', KEYS[1], ARGV[1])

if status then
    -- Seat is already taken
    return 0
else
    -- Seat is free, book it
    redis.call('HSET', KEYS[1], ARGV[1], ARGV[2])
    return 1
end