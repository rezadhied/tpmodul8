using System;
using System.IO;
using Newtonsoft.Json;

class CovidConfig{
    public string SatuanSuhu { get; set; }
    public int BatasHariDemam { get; set; }
    public string PesanDitolak { get; set; }
    public string PesanDiterima { get; set; }

    public CovidConfig()
    {
        SatuanSuhu = "celcius";
        BatasHariDemam = 14;
        PesanDitolak = "Anda tidak diperbolehkan masuk kedalam gedung ini";
        PesanDiterima = "Anda dipersilahkan untuk masuk ke dalam gedung ini";
    }

    public void ReadJSON()
    {
        try
        {
            string jsonString = File.ReadAllText("covid_config.json");
            CovidConfig config = JsonConvert.DeserializeObject<CovidConfig>(jsonString);
            SatuanSuhu = config.SatuanSuhu;
            BatasHariDemam = config.BatasHariDemam;
            PesanDitolak = config.PesanDitolak;
            PesanDiterima = config.PesanDiterima;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to read configuration file: " + ex.Message);
        }
    }

    public void WriteJSON()
    {
        try
        {
            string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText("covid_config.json", jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to write configuration file: " + ex.Message);
        }
    }

    public void UbahSatuan()
    {
        if (SatuanSuhu == "celcius")
        {
            SatuanSuhu = "fahrenheit";
        }
        else if (SatuanSuhu == "fahrenheit")
        {
            SatuanSuhu = "celcius";
        }

        WriteJSON();
    }
}

class program
{
    static void Main(string[] args)
    {
        CovidConfig config = new CovidConfig();
        config.ReadJSON();

        Console.Write($"Berapa suhu badan anda saat ini? Dalam nilai {config.SatuanSuhu}: ");
        double suhu;
        while (!double.TryParse(Console.ReadLine(), out suhu))
        {
            Console.Write("Masukan tidak valid. Silakan coba lagi: ");
        }

        bool suhuValid = false;
        if (config.SatuanSuhu == "celcius")
        {
            suhuValid = suhu >= 36.5 && suhu <= 37.5;
        }
        else if (config.SatuanSuhu == "fahrenheit")
        {
            suhuValid = suhu >= 97.7 && suhu <= 99.5;
        }

        Console.Write($"Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala demam? ");
        int hari;
        while (!int.TryParse(Console.ReadLine(), out hari))
        {
            Console.Write("Masukan tidak valid. Silakan coba lagi: ");
        }

        if (suhuValid && hari < config.BatasHariDemam)
        {
            Console.WriteLine(config.PesanDiterima);
        }
        else
        {
            Console.WriteLine(config.PesanDitolak);
        }

        config.UbahSatuan();
    }
}