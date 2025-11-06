# Værksted Management App

Dette er en cross-platform applikation udviklet med .NET MAUI, designet til at hjælpe bilværksteder med effektivt at administrere kundeordrer og fakturering.

## Funktioner

Applikationen strømliner den daglige drift på et værksted med følgende kernefunktioner:

1.  **Ordrestyring:**
    *   Opret nemt nye serviceordrer med kundeoplysninger, køretøjsinformation og arbejdsbeskrivelser.
    *   Hver ordre tildeles automatisk et unikt ordrenummer for nem sporing.

2.  **Kalenderoversigt:**
    *   Få et klart overblik over planlagte opgaver for enhver valgt dato, hvilket hjælper med daglig planlægning og ressourceallokering.

3.  **Fakturering:**
    *   Generer detaljerede fakturaer baseret på afsluttede ordrer.
    *   Tilføj medgåede arbejdstimer, mekanikeroplysninger og en specificeret liste over anvendte materialer.

4.  **Fakturaoversigt:**
    *   Få adgang til og gennemgå en komplet liste over alle genererede fakturaer.
    *   Inkluderer søgefunktionalitet for hurtigt at finde specifikke dokumenter.

## Teknisk Arkitektur

Projektet er bygget på en moderne teknisk stak for at sikre robusthed og vedligeholdelse:

*   **Platform:** .NET MAUI, som muliggør en enkelt kodebase for flere platforme.
*   **Sprog:** C#
*   **Arkitektur:** Anvender Model-View-ViewModel (MVVM) mønsteret for en ren adskillelse af UI og forretningslogik.
*   **Toolkit:** Benytter `CommunityToolkit.Mvvm` til en effektiv og moderne implementering af MVVM.
*   **Database:** Alle data persisteres lokalt i en SQLite-database.
*   **Dependency Injection:** Udnytter .NET MAUI's indbyggede container til at håndtere afhængigheder i hele applikationen.

## Projektstruktur

Projektet er logisk organiseret for at fremme en overskuelig og skalerbar kodebase:

*   **/Data**: Håndterer al databaseinteraktion, herunder opsætning og forbindelse via `AppDatabase.cs`.
*   **/Models**: Indeholder datamodellerne (`Order.cs`, `Invoice.cs`, `InvoiceItem.cs`), der definerer applikationens dataobjekter.
*   **/ViewModels**: Indeholder logikken og tilstands-håndteringen for hver visning, hvilket skaber et bindeled mellem data og brugergrænsefladen.
*   **/Views**: Består af XAML-filer og deres code-behind, som definerer applikationens brugergrænseflade.

## Sådan kører du projektet

1.  Klon dette repository til din lokale maskine.
2.  Åbn løsningsfilen (`Vaerksted.sln`) i Visual Studio.
3.  Gendan NuGet-pakkerne for at installere alle projektets afhængigheder.
4.  Vælg den ønskede startplatform (f.eks. Windows Machine, Android Emulator, iOS Simulator).
5.  Tryk på Start-knappen (eller F5) for at bygge og køre applikationen.
