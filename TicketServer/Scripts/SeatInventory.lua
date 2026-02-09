-- KEYS[1]: master_flight_key
-- KEYS[2]: reserved_flight_key
-- ARGV[1]: seat_field
-- ARGV[2]: user_id value to set if the seat is booked

local isValidSeat = redis.call('SISMEMBER', KEYS[1], ARGV[1])

-- Step 1: Validate if the seat is valid (exists in master flight key)
if isValidSeat == 0 then
    return {-1, "Seat is not valid"}
end

-- Step 2: Check if the seat is already reserved in the reserved flight key
local isOccupied = redis.call('HGET', KEYS[2], ARGV[1])

if isOccupied then
    return {0, isOccupied}
else
    redis.call('HSET', KEYS[1], ARGV[1], ARGV[2])
    return {1, "Seat booked successfully"}
end