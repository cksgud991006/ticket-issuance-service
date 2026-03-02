-- KEYS[1]: available_count_flight_key
-- KEYS[2]: master_flight_key
-- KEYS[3]: reserved_flight_key
-- ARGV[1]: seat_field
local availableCount = redis.call('GET', KEYS[1])
local count = tonumber(availableCount)
if count < 1 then
    return {-2, "No seats available"}
end


local isValidSeat = redis.call('SISMEMBER', KEYS[2], ARGV[1])

-- Step 1: Validate if the seat is valid (exists in master flight key)
if isValidSeat == 0 then
    return {-1, "Seat is not valid"}
end

-- Step 2: Check if the seat is already reserved in the reserved flight key
local isOccupied = redis.call('SISMEMBER', KEYS[3], ARGV[1])

if isOccupied == 1 then
    return {0, "Seat is already occupied"}
else
    redis.call('SADD', KEYS[3], ARGV[1])
    redis.call('DECR', KEYS[1])
    return {1, "Seat booked successfully"}
end