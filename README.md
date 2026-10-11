# Car Dealership App (Oto Galeri)

[![build](https://github.com/theTEK0000/Car-Dealership-App/actions/workflows/dotnet.yml/badge.svg)](https://github.com/theTEK0000/Car-Dealership-App/actions/workflows/dotnet.yml)
![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4)
![C#](https://img.shields.io/badge/C%23-console-239120)

A console application for a small car dealership that rents out vehicles: add and remove cars, rent and return them, cancel a rental and see fleet statistics and revenue at a glance. Written in C# (.NET 8), with xUnit tests. The UI is in Turkish.

## Features

- Add, remove and list cars (all / rented / in the showroom)
- Rent a car for a number of hours and take it back
- Cancel an ongoing rental
- Fleet statistics: total cars, rented vs. available, total rental hours and count
- Revenue: every started day is billed as a full day (25 h = 2 days)
- Turkish licence plate validation: province code 01–81 and all valid letter/number layouts (`34 A 1234`, `06 AB 123`, `07 ABC 12` …); spaces and lower case are accepted

## Example

```
Galeri Otomasyon
1- Araba Kirala (K)
2- Araba Teslim Al (T)
3- Kiradaki Arabaları Listele (R)
4- Galerideki Arabaları Listele (M)
5- Tüm Arabaları Listele (A)
6- Kiralama İptali (I)
7- Araba Ekle (Y)
8- Araba Sil (S)
9- Bilgileri Göster (G)
X- Çıkış

Seçiminiz: a

-Tüm Arabalar-

Plaka      Marka         TL/gün Tip        K.Sayısı  Durum
--------------------------------------------------------------
07ASD123   Renault          900 Hatchback         1  Kirada
07QWE123   Fiat           1.100 Sedan             0  Galeride
07ZXC123   Toyota         1.300 SUV               0  Galeride
```

## Run

Requires the [.NET 8 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet run --project src/OtoGaleri
dotnet test
```

## Project structure

```
src/OtoGaleri/           Console app
  Araba.cs               Car model, per-car revenue
  Galeri.cs              Dealership logic (rent, return, cancel, add, remove, stats)
  Plaka.cs               Plate normalisation and validation
  Program.cs             Menu and console input/output
tests/OtoGaleri.Tests/   xUnit tests for the logic and plate rules
```

The business logic in `Galeri` has no console code, so it is tested directly.

## License

[MIT](LICENSE)
