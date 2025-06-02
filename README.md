# XmlCore

## 🛒 Artikel- och Orderhantering – ASP.NET Core Blazor
##📌 Beskrivning
Denna applikation hanterar artiklar och ordrar genom ett gränssnitt där användaren kan skapa artiklar, lägga ordrar och söka efter befintliga beställningar. Databasen används för att lagra all information och följer databasnormalisering för optimal prestanda.

Applikationen är byggd i ASP.NET Core Blazor med .NET 8 och använder Microsoft SQL Server för datalagring. Entity Framework hanterar objektrelationell avbildning (ORM).

## 🔧 Funktioner
📦 Artikelhantering
- Skapa nya artiklar med artikelnummer, namn, styckpris och en färg för visuell identifiering.
- Artiklar lagras i databasen och kan redigeras.

### 🛍️ Orderhantering
- Användare kan lägga ordrar som sparas i databasen.
En order innehåller:
- Ordernummer
- Kundinformation (exempelvis kundnamn)
- Artikellista (artikelnamn, styckpris, antal och totalsumma)

### 🔎 Sökfunktion
- Sök efter en specifik artikel och se alla ordrar som innehåller den.
- Informationen om ordern presenteras strukturerat i gränssnittet.

### 🏗️ XML-hantering
- Exportera en order till XML-format.
- Importera en order via XML, där systemet:
- Skapar företaget som en ny användare om det inte finns.
- Lägger till produkterna i databasen om de saknas.

### 👤 Användarroller
- Kunder kan skapa ordrar och söka efter produkter.
- Admin hanterar hela applikationen.
- Konto-registrering för att skapa en personlig profil.

## 🛠️ Tekniker och Verktyg
- ASP.NET Core Blazor (Frontend och Backend)
- .NET 8
- Microsoft SQL Server (Databashantering)
- Entity Framework (ORM)
- Git (Versionshantering)
- Jira (Ärendehantering)
