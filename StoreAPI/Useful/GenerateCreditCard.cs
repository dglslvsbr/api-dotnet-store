using StoreAPI.Entities.Models;

namespace StoreAPI.Useful;

public static class GenerateCreditCard
{
    public static CreditCard Generate()
    {
        Random random = new();

        string number = "";
        while (number.Length < 16)
            number += random.Next(1, 9);

        string cvv = "";
        while (cvv.Length < 3) 
            cvv += random.Next(1, 9);

        string limit = "";
        while (limit.Length < 4)
            limit += random.Next(1, 9);

        return new CreditCard()
        {
            Number = number,
            Expiration = DateTimeOffset.Now.AddYears(5),
            CVV = cvv,
            UsedLimit = 0.0m,
            MaxLimit = decimal.Parse(limit)
        };
    }
}