using StoreAPI.Entities.Models;

namespace StoreAPI.Useful;

public static class ProductHasData
{
    public static List<Product> Products()
    {
        return [new Product
        {
            Id = 1,
            Name = "RTX 4070 Ti",
            Description = "It is a video card with excellent cost-effectiveness, extremely efficient and capable of delivering better performance in games.",
            ImageUrl = "assets/images/GraphicCard4070Ti.png",
            Price = 7500m,
            CategoryId = 1
        },
        new Product
        {
            Id = 2,
            Name = "RTX 5090",
            Description = "It is a video card with excellent cost-effectiveness, extremely efficient and capable of delivering better performance in games.",
            ImageUrl = "assets/images/GraphicCard5090.png",
            Price = 20999m,
            CategoryId = 1
        },
        new Product
        {
            Id = 3,
            Name = "PlayStation 5",
            Description = "The latest console from Sony, more efficient than previous generations, delivering maximum performance and excellent graphics.",
            ImageUrl = "assets/images/PlayStation5.png",
            Price = 3299m,
            CategoryId = 2
        },
        new Product
        {
            Id = 4,
            Name = "PlayStation 4",
            Description = "Console from Sony, more efficient than previous generations, delivering maximum performance and excellent graphics.",
            ImageUrl = "assets/images/PlayStation4.png",
            Price = 2299m,
            CategoryId = 2
        },
        new Product
        {
            Id = 5,
            Name = "Redmi Note 13",
            Description = "It is an excellent cost-effective cell phone, currently considered the best of today, with a 150mpx camera.",
            ImageUrl = "assets/images/XiaomiRedmiNote13.png",
            Price = 3599m,
            CategoryId = 3
        },
        new Product
        {
            Id = 6,
            Name = "Iphone 14",
            Description = "It is an excellent cost-effective cell phone, currently considered the best of today, with a 150mpx camera.",
            ImageUrl = "assets/images/Iphone14.png",
            Price = 6500m,
            CategoryId = 3
        },
        new Product
        {
            Id = 7,
            Name = "AMD Ryzen 5 5500",
            Description = "Can deliver fast performance of over 100 FPS in the world's most popular games, discrete graphics required",
            ImageUrl = "assets/images/Processor5500.png",
            Price = 590m,
            CategoryId = 1
        },
        new Product
        {
            Id = 8,
            Name = "AMD Ryzen 9 5950X",
            Description = "The best processor for gamers meets the best processor for creators, with 16 cores and 32 processing lines",
            ImageUrl = "assets/images/Processor5950X.png",
            Price = 3999m,
            CategoryId = 1
        },
        new Product
        {
            Id = 9,
            Name = "AMD Ryzen 7 5700X",
            Description = "Can deliver ultra-fast 100 FPS performance in the world's most popular games, discrete graphics card required",
            ImageUrl = "assets/images/Processor5700X.png",
            Price = 1089m,
            CategoryId = 1
        },
        new Product
        {
            Id = 10,
            Name = "The Last of Us",
            Description = "Winner of over 300 Game of the Year awards, remastered for the PS5 console.",
            ImageUrl = "assets/images/TheLastOfUs2.png",
            Price = 220m,
            CategoryId = 4
        },
        new Product
        {
            Id = 11,
            Name = "God of War",
            Description = "Embark on an epic and heartbreaking journey where Kratos and Atreus struggle between the desire to stay together or separate.",
            ImageUrl = "assets/images/GowRagnarok.png",
            Price = 220m,
            CategoryId = 4
        },
        new Product
        {
            Id = 12,
            Name = "PlayStation 5 Gold",
            Description = "Sony's latest limited edition console, more efficient than previous generations, offering maximum performance and excellent graphics.",
            ImageUrl = "assets/images/PlayStation5Edition.png",
            Price = 4899m,
            CategoryId = 2
        },
        new Product
        {
            Id = 13,
            Name = "Console Retrô PSX",
            Description = "All the Classics You Love, Plus 93 Thousand Titles!",
            ImageUrl = "assets/images/ConsoleRetroPSX.png",
            Price = 2399m,
            CategoryId = 2
        },
        new Product
        {
            Id = 14,
            Name = "Elden Ring",
            Description = "The Golden Order has been broken. Rise, Tarnished, and be guided by grace to wield the power of the Pristine Ring and become a Pristine Lord in the Lands Between.",
            ImageUrl = "assets/images/EldenRing.png",
            Price = 220m,
            CategoryId = 4
        },
        new Product
        {
            Id = 15,
            Name = "PlayStation Portátil",
            Description = "Your PS5 in the palm of your hand with the PlayStation Portal Remote Player",
            ImageUrl = "assets/images/PlayStationPortatil.png",
            Price = 4100m,
            CategoryId = 2
        },
        new Product
        {
            Id = 16,
            Name = "Resident Evil 4",
            Description = "Resident Evil 4 is a remake of the original 2005 game. With revamped graphics, updated gameplay, and a reimagined storyline, while preserving the essence of the original game.Resident Evil 4 is a remake of the original 2005 game. With revamped graphics, updated gameplay, and a reimagined storyline, while preserving the essence of the original game.",
            ImageUrl = "assets/images/ResidentEvil4.png",
            Price = 220m,
            CategoryId = 4
        },
        new Product
        {
            Id = 17,
            Name = "Miles Morales",
            Description = "Experience the rise of Miles Morales as the hero who masters the new, amazing, and explosive powers to become Spider-Man himself",
            ImageUrl = "assets/images/SpiderManMilesMorales.png",
            Price = 220m,
            CategoryId = 4
        },
        new Product
        {
            Id = 18,
            Name = "Motorola G15",
            Description = "50 MP main camera with AI and Automatic Night Vision",
            ImageUrl = "assets/images/MotorolaG15.png",
            Price = 889m,
            CategoryId = 3
        },
        new Product
        {
            Id = 19,
            Name = "Samsung Galaxy S25",
            Description = "Multiple actions in a single voice command",
            ImageUrl = "assets/images/SamsungGalaxyS25.png",
            Price = 3999m,
            CategoryId = 3
        },
        new Product
        {
            Id = 20,
            Name = "Xiaomi Poco X17",
            Description = "Processing and screen to compete and win. New Mediatek Dimensity 8400-Ultra processor and 1.5 curved AMOLED screen with 120Hz, your new high-performance mobile gaming setup. Express your creativity and share your stories with quality.",
            ImageUrl = "assets/images/XiaomiPocoX17.png",
            Price = 2199m,
            CategoryId = 3
        },
        new Product
        {
            Id = 21,
            Name = "Notebook",
            Description = "Equipped with the powerful 13th-generation Intel Core i7-13620H processor and the NVIDIA GeForce RTX 3050 graphics card, this laptop delivers the performance you need to tackle any challenge.",
            ImageUrl = "assets/images/NotebookAcerNitro.png",
            Price = 4599m,
            CategoryId = 1
        },
        new Product
        {
            Id = 22,
            Name = "Mouse Branco",
            Description = "Wireless gaming mouse Attack Shark X11 white – lightweight, precision, and advanced connectivityThe Attack Shark X11 white is the ideal choice for those seeking professional performance combined with a lightweight and modern design.",
            ImageUrl = "assets/images/MouseBranco.png",
            Price = 189m,
            CategoryId = 1
        },
        new Product
        {
            Id = 23,
            Name = "Monitor",
            Description = "If you are looking for an immersive experience for various types of games, such as strategy, FPS, and racing, this monitor is perfect for you.",
            ImageUrl = "assets/images/MonitorUltraGear.png",
            Price = 1459m,
            CategoryId = 1
        },
        new Product
        {
            Id = 24,
            Name = "HeadsetRedragon",
            Description = "Experience a new dimension of audio with the Redragon Zeus X gaming headset. Its 53mm drivers deliver rich and powerful sound, with deep bass and crystal-clear highs. With 7.1 virtual surround sound, you will hear every detail of the game, from enemy footsteps to the most intense explosions.",
            ImageUrl = "assets/images/HeadsetRedragon.png",
            Price = 199m,
            CategoryId = 1
        }];
    }
}