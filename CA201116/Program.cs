using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CA201116
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            // Feladat leírása
            /*
             * Az egitestek.txt fájlban a Naprendszer égitesteinek adatai találhatók, soronként a következő adatokkal:
             * az égitest neve, annak az égitestnek a neve, ami körül kering, a keringési távolság,
             * az égitest átmérője. Az egy sorban szereplő különböző adatokat szóköz választja el.
             * 
             * Olvasd be a fájl tartalmát, majd írd a képernyőre a következő kérdésekre adott válaszokat:
             * a) Mekkora az égitestek átlagos mérete?
             * b) Melyik a legnagyobb átmérőjű égitest?
             * c) Hány holdja van a Neptunusznak (azaz hány égitest kering körülötte)?
             * d) Melyek a Naprendszerünk bolygói (mely égitestek keringenek a Nap körül)?
             * e) Válogasd ki egy listába a Jupiter holdjait, majd rendezd a tömböt az égitestek keringési
             * távolsága szerint növekvő sorrendbe és írasd ki a jupiter.txt fájlba.
             */
            
            var data = new Dictionary<string, Dictionary<string, object>>();

            using (var sr = new StreamReader(@"..\..\Resources\egitestek.txt", Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var split = line.Split(' ');
                    data.Add(split[0], new Dictionary<string, object>
                    {
                        {"mkk", split[1]},
                        {"kerTav", Convert.ToInt64(split[2])},
                        {"atmero", Convert.ToInt32(split[3])}
                    });
                }
            }
            Console.WriteLine("Az egitestek.txt tartalma beolvasva.\n");

            // a) feladat
            var atmerok = new int[data.Count];
            for (var i = 0; i < atmerok.Length; i++) atmerok[i] = (int) data.ElementAt(i).Value["atmero"];
            Console.WriteLine($"Átlagos átmérőjük: {atmerok.Average()}");
            
            // b) feladat
            var maxAtmEgitest = data.ElementAt(Array.FindIndex(atmerok, i => i == atmerok.Max())).Key;
            Console.WriteLine($"A legnagyobb átmérőjű égitest: {maxAtmEgitest}");
            
            // c) feladat
            var countNep = data.Values.Count(val => (string) val["mkk"] == "Neptunusz");
            Console.WriteLine($"Neptunusz körül keringő égitestek száma: {countNep}");
            
            // d) feladat
            var naprendszer = new List<string>();
            foreach (var pair in data) if ((string) pair.Value["mkk"] == "Nap") naprendszer.Add(pair.Key);
            Console.WriteLine($"Naprendszerünk bolygói (a Napon kívül):\n{string.Join(", ", naprendszer)}");
            
            // e) feladat
            var jupiterHoldak = data.Where(pair => (string) pair.Value["mkk"] == "Jupiter")
                .ToDictionary(pair => pair.Key, pair => pair.Value).ToList();
            jupiterHoldak.Sort((pair1, pair2) => 
                ((long) pair1.Value["kerTav"]).CompareTo((long) pair2.Value["kerTav"]));
            
            // e) feladat fájlba írás
            var jupLines = jupiterHoldak.Select(el => 
                $"{el.Key} {el.Value["mkk"]} {el.Value["kerTav"]} {el.Value["atmero"]}")
                .ToList();
            using (var sw = new StreamWriter("jupiter.txt")) 
                foreach (var line in jupLines) sw.WriteLine(line);

            Console.WriteLine("\nA jupiter.txt fájlba írás megtörtént.");
            
            Console.ReadKey(true);
        }
    }
}