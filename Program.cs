using System;
using System.Collections.Generic;
using System.Linq;

class ParkingLot
{
    private int totalSlots;
    private List<string[]> slots; 

    public ParkingLot(int size)
    {
        totalSlots = size;
        slots = new List<string[]>(new string[totalSlots][]);
        Console.WriteLine($"Created a parking lot with {totalSlots} slots");
    }

    public void ParkVehicle(string registrationNo, string color, string type)
    {
        for (int i = 0; i < totalSlots; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = new string[] { registrationNo, color, type }; 
                Console.WriteLine($"Allocated slot number: {i + 1}");
                return;
            }
        }
        Console.WriteLine("Sorry, parking lot is full");
    }

    public void LeaveVehicle(int slotNumber)
    {
        if (slotNumber < 1 || slotNumber > totalSlots)
        {
            Console.WriteLine("Invalid slot number");
            return;
        }

        if (slots[slotNumber - 1] != null)
        {
            slots[slotNumber - 1] = null;
            Console.WriteLine($"Slot number {slotNumber} is free");
        }
        else
        {
            Console.WriteLine($"Slot number {slotNumber} is already empty");
        }
    }

    public void DisplayStatus()
    {
        Console.WriteLine("Slot\tNo.\t\tType\tRegistration No\tColour");
        bool isEmpty = true;

        for (int i = 0; i < totalSlots; i++)
        {
            if (slots[i] != null)
            {
                string[] details = slots[i];
                Console.WriteLine($"{i + 1}\t{details[0]}\t{details[2]}\t{details[1]}");
                isEmpty = false;
            }
        }

        if (isEmpty)
        {
            Console.WriteLine("Parking lot is empty.");
        }
    }

    public void CountVehiclesByType(string type)
    {
        int count = 0;

        for (int i = 0; i < totalSlots; i++)
        {
            if (slots[i] != null)
            {
                string[] details = slots[i];
                if (details[2].Equals(type, StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                }
            }
        }

        Console.WriteLine(count);
    }

    public void RegistrationNumbersWithOddPlates()
    {
        var oddPlates = slots
            .Where(slot => slot != null && IsOddPlate(slot[0]))
            .Select(slot => slot[0]);

        string result = string.Join(", ", oddPlates);
        Console.WriteLine(result);
    }

    public void RegistrationNumbersWithEventPlates()
    {
        var eventPlates = slots
            .Where(slot => slot != null && IsEventPlate(slot[0]))
            .Select(slot => slot[0]);

        string result = string.Join(", ", eventPlates);
        Console.WriteLine(result);
    }


    private bool IsOddPlate(string registrationNo)
    {
        var middleNumber = ExtractMiddleNumber(registrationNo);
        return middleNumber % 2 != 0;
    }

    private bool IsEventPlate(string registrationNo)
    {
        var middleNumber = ExtractMiddleNumber(registrationNo);
        return middleNumber % 2 == 0;
    }

    private int ExtractMiddleNumber(string registrationNo)
    {
        var parts = registrationNo.Split('-');
        if (parts.Length < 2 || !int.TryParse(parts[1], out int middleNumber))
        {
            return 0;
        }

        return middleNumber;
    }

    public void RegistrationNumbersWithColour(string colour)
    {

        var plates = slots
            .Where(slot => slot != null && slot[1].Equals(colour, StringComparison.OrdinalIgnoreCase)) 
            .Select(slot => slot[0]); 

        string result = string.Join(", ", plates);
        Console.WriteLine(result); 
    }

    public void SlotNumbersWithColour(string colourWithSlotNumber)
    {

        var slotNumbers = slots
            .Select((slot, index) => new { Slot = slot, Index = index })
            .Where(item => item.Slot != null && item.Slot[1].Equals(colourWithSlotNumber, StringComparison.OrdinalIgnoreCase))
            .Select(item => item.Index + 1);

        string result = string.Join(", ", slotNumbers);

        Console.WriteLine(result);
    }

    public void SlotNumberForRegistrationNumber(string registrationNo)
    {
        var slotNumber = slots
            .Select((slot, index) => new { Slot = slot, Index = index })
            .Where(item => item.Slot != null && item.Slot[0].Equals(registrationNo, StringComparison.OrdinalIgnoreCase))
            .Select(item => item.Index + 1) 
            .FirstOrDefault();

        if (slotNumber > 0)
        {
            Console.WriteLine(slotNumber);
        }
        else
        {
            Console.WriteLine("Not Found");
        }
    }



}

class Program
{
    static void Main(string[] args)
    {
        ParkingLot parkingLot = null; 
        while (true)
        {
            string command = Console.ReadLine();
            string[] commandParts = command.Split(' ');

            switch (commandParts[0])
            {
                case "create_parking_lot":
                    int size = int.Parse(commandParts[1]);
                    parkingLot = new ParkingLot(size);
                    break;

                case "park":
                    
                    if (commandParts.Length < 4)
                    {
                        Console.WriteLine("Invalid input. Please provide registration number, color, and type.");
                        break;
                    }
                    string registrationNo = commandParts[1];
                    string color = commandParts[2];
                    string type = commandParts[3];
                    parkingLot.ParkVehicle(registrationNo, color, type);
                    break;

                case "leave":
                   
                    if (commandParts.Length < 2 || !int.TryParse(commandParts[1], out int slotNumber))
                    {
                        Console.WriteLine("Invalid input. Please provide a valid slot number.");
                        break;
                    }
                    parkingLot.LeaveVehicle(slotNumber);
                    break;

                case "status":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot has not been created yet.");
                        break;
                    }
                    parkingLot.DisplayStatus();
                    break;

                case "type_of_vehicles":
                    
                    string vehicleType = commandParts[1];
                    parkingLot.CountVehiclesByType(vehicleType);
                    break;

                case "registration_numbers_for_vehicles_with_ood_plate":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot has not been created yet.");
                        break;
                    }
                    parkingLot.RegistrationNumbersWithOddPlates();
                    break;

                case "registration_numbers_for_vehicles_with_event_plate":
                    
                    parkingLot.RegistrationNumbersWithEventPlates();
                    break;

                case "registration_numbers_for_vehicles_with_colour":
                    
                    string colour = commandParts[1];
                    parkingLot.RegistrationNumbersWithColour(colour);
                    break;

                case "slot_numbers_for_vehicles_with_colour":
                    
                    string colourWithSlotNumber = commandParts[1];
                    parkingLot.SlotNumbersWithColour(colourWithSlotNumber);
                    break;

                case "slot_number_for_registration_number":
                    
                    string regNo = commandParts[1];
                    parkingLot.SlotNumberForRegistrationNumber(regNo);
                    break;



                case "exit":
                    return;

                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
        }
    }
}
