# Sachssoft Sasodoc

Sasodoc ist eine leichtgewichtige, erweiterbare und AOT-freundliche Bibliothek zur Dokumentformatierung und Serialisierung für .NET.

Sie stellt eine gemeinsame API zum Lesen und Schreiben verschiedener Dokumentformate bereit, während formatspezifische Implementierungen in eigenständigen Paketen getrennt bleiben.

> [!WARNING]
> Sasodoc 2.0 führt gegenüber früheren Versionen umfangreiche API- und Architekturänderungen ein.
> Einige Formatpakete befinden sich noch in aktiver Entwicklung und können weiterhin Breaking Changes erhalten.

## Dokumentation

* **Anleitung zum Erstellen und Veröffentlichen von Paketen**: Siehe [PackageBuildInstructions.md](PackageBuildInstructions.md)
* **Lizenz**: Siehe [LICENSE.md](LICENSE.md)
* **Changelog**: Siehe [CHANGELOG.md](CHANGELOG.md)
* **Issues & Entwicklung**: Siehe [GitHub Issues](https://github.com/sachssoft/Sasodoc/issues)

## Voraussetzungen

* .NET 8.0
* .NET 9.0
* .NET 10.0
* Trimming-kompatibel
* Native-AOT-kompatibel

## Pakete

> [!IMPORTANT]
> **Verfügbarkeit** und **Stabilität** sind voneinander unabhängig.
>
> **Verfügbar** bedeutet, dass das Paket veröffentlicht wurde und verwendet werden kann.
>
> **Stabil** bedeutet, dass die Implementierung als ausreichend ausgereift für den regulären Einsatz betrachtet wird.
>
> **Instabile** Pakete sind verfügbar und können verwendet werden, können jedoch noch unvollständige Funktionen oder bekannte Probleme enthalten sowie größeren Änderungen unterliegen.
>
> Bekannte Probleme, geplante Verbesserungen und die laufende Entwicklung sind unter [GitHub Issues](https://github.com/sachssoft/Sasodoc/issues) zu finden.

| Paket              | Verwendung             | Verfügbarkeit | Stabilität | NuGet                                                                                                                            |
| ------------------ | ---------------------- | ------------- | ---------- | -------------------------------------------------------------------------------------------------------------------------------- |
| **Sasodoc**        | Zentrale Dokument-API  | Verfügbar     | Stabil     | [![NuGet](https://img.shields.io/nuget/v/Sachssoft.Sasodoc.svg)](https://www.nuget.org/packages/Sachssoft.Sasodoc)               |
| **Sasodoc.Json**   | JSON-Dokumentformat    | Verfügbar     | Stabil     | [![NuGet](https://img.shields.io/nuget/v/Sachssoft.Sasodoc.Json.svg)](https://www.nuget.org/packages/Sachssoft.Sasodoc.Json)     |
| **Sasodoc.Xml**    | XML-Dokumentformat     | Verfügbar     | Instabil   | [![NuGet](https://img.shields.io/nuget/v/Sachssoft.Sasodoc.Xml.svg)](https://www.nuget.org/packages/Sachssoft.Sasodoc.Xml)       |
| **Sasodoc.Ini**    | INI-Dokumentformat     | Verfügbar     | Instabil   | [![NuGet](https://img.shields.io/nuget/v/Sachssoft.Sasodoc.Ini.svg)](https://www.nuget.org/packages/Sachssoft.Sasodoc.Ini)       |
| **Sasodoc.Yaml**   | YAML-Dokumentformat    | Verfügbar     | Instabil   | [![NuGet](https://img.shields.io/nuget/v/Sachssoft.Sasodoc.Yaml.svg)](https://www.nuget.org/packages/Sachssoft.Sasodoc.Yaml)     |
| **Sasodoc.Binary** | Binäres Dokumentformat | Verfügbar     | Instabil   | [![NuGet](https://img.shields.io/nuget/v/Sachssoft.Sasodoc.Binary.svg)](https://www.nuget.org/packages/Sachssoft.Sasodoc.Binary) |

> [!NOTE]
> Der Stabilitätsstatus beschreibt den aktuellen Reifegrad eines Pakets und kann sich unabhängig von seiner Verfügbarkeit ändern.
> Vor einem produktiven Einsatz sollten die aktuellen [GitHub Issues](https://github.com/sachssoft/Sasodoc/issues) geprüft werden, insbesondere bei Paketen, die als **Instabil** gekennzeichnet sind.

## Funktionen

* Gemeinsame formatunabhängige Reader- und Writer-API
* Unterstützung für JSON-Dokumente
* Unterstützung für XML-Dokumente
* Unterstützung für INI-Dokumente
* Unterstützung für YAML-Dokumente
* Unterstützung für Binärdokumente
* Primitive Werte und Arrays
* Verschachtelte Objekte
* Objekt-Arrays
* Benutzerdefinierte Wertkonverter
* Konfigurierbare Namenskonventionen
* Formatunabhängige Datenbezeichner
* Synchrone Stream-Ein-/Ausgabe
* Asynchrone Stream-Ein-/Ausgabe
* Trimming-Unterstützung
* Native-AOT-Unterstützung
* Multi-Targeting für .NET 8, .NET 9 und .NET 10
* Separate Pakete für einzelne Dokumentformate
* Geringe Abhängigkeiten

## Dokumentformate

### JSON

JSON-Unterstützung wird durch `Sachssoft.Sasodoc.Json` bereitgestellt.

JSON gehört zu den primären und stabilen Dokumentformaten von Sasodoc.

```csharp
using Sachssoft.Documents.Formats.Json;
```

NuGet:

```text
Sachssoft.Sasodoc.Json
```

### XML

XML-Unterstützung wird durch `Sachssoft.Sasodoc.Xml` bereitgestellt.

Der XML-Formatter verwendet XML-Attribute für primitive Eigenschaften und untergeordnete Elemente für verschachtelte Objekte.

Beispiel:

```xml
<Object name="Test" enabled="true" count="5">
    <transform x="10" y="20" />
</Object>
```

```csharp
using Sachssoft.Documents.Formats.Xml;
```

NuGet:

```text
Sachssoft.Sasodoc.Xml
```

### INI

INI-Unterstützung wird durch `Sachssoft.Sasodoc.Ini` bereitgestellt.

Primitive Werte werden als Schlüssel-Wert-Paare dargestellt, während verschachtelte Objekte über Sektionen abgebildet werden können.

Beispiel:

```ini
name=Test
enabled=true
count=5

[player]
name=Tobias

[player.position]
x=100
y=200
```

```csharp
using Sachssoft.Documents.Formats.Ini;
```

NuGet:

```text
Sachssoft.Sasodoc.Ini
```

### YAML

YAML-Unterstützung wird durch `Sachssoft.Sasodoc.Yaml` bereitgestellt.

Beispiel:

```yaml
name: Test
enabled: true

player:
  name: Tobias
  position:
    x: 100
    y: 200

numbers:
  - 10
  - 20
  - 30
```

```csharp
using Sachssoft.Documents.Formats.Yaml;
```

NuGet:

```text
Sachssoft.Sasodoc.Yaml
```

### Binary

Die Unterstützung für Binärdokumente wird durch `Sachssoft.Sasodoc.Binary` bereitgestellt.

Der Binary-Formatter bietet mehrere Strategien zur Speicherung von Dokumentbezeichnern und ermöglicht die Konfiguration der binären Darstellung abhängig von Anforderungen an Kompatibilität und Dateigröße.

Unterstützte Identifier-Modi:

* Keine gespeicherten Identifier
* Inline-Identifier
* Identifier-Registry

Die Länge von Identifiern kann mit fester oder variabler Längenkodierung gespeichert werden.

Das Binärformat benötigt außerdem zwingend einen Magic-Wert. Dadurch können Anwendungen Binärdokumente identifizieren und validieren.

Zu den vorgesehenen Magic-Werten gehören:

* `SDB`
* `BIN`
* `DAT`
* `SASODOCBIN`
* `SASODOCDAT`

Übliche Dateiendungen:

* `.sdb` — Sasodoc Binary
* `.bin`
* `.dat`

```csharp
using Sachssoft.Documents.Formats.Binary;
```

NuGet:

```text
Sachssoft.Sasodoc.Binary
```

## Architektur

Sasodoc trennt die formatunabhängige Dokument-API von den Implementierungen der einzelnen Dokumentformate.

Das Core-Paket stellt die gemeinsame Reader-, Writer-, Formatierungs-, Naming- und Serialisierungsinfrastruktur bereit.

Formatspezifische Implementierungen werden als separate Pakete veröffentlicht.

```text
Sachssoft.Sasodoc
│
├── Sachssoft.Sasodoc.Json
├── Sachssoft.Sasodoc.Xml
├── Sachssoft.Sasodoc.Ini
├── Sachssoft.Sasodoc.Yaml
└── Sachssoft.Sasodoc.Binary
```

Anwendungen müssen dadurch nur die Dokumentformate referenzieren, die sie tatsächlich verwenden.

## Datenbezeichner

Sasodoc verwendet `DataIdentifier` zur Darstellung formatunabhängiger Datenbezeichner.

Ein Identifier besteht aus einem textuellen Wert und einer semantischen Identifier-Art.

Dadurch können Formate zwischen normalen Eigenschaften und speziellen Identifiern unterscheiden, ohne formatspezifische Schlüsseltypen über die gemeinsame API offenzulegen.

```csharp
DataIdentifier identifier = new("name");
```

Strings können außerdem implizit konvertiert werden:

```csharp
DataIdentifier identifier = "name";
```

Spezielle Identifier-Arten können verwendet werden, wenn ein Identifier eine semantische Bedeutung besitzt und nicht wie ein normaler Eigenschaftsname behandelt werden soll.

## Namenskonventionen

Sasodoc unterstützt konfigurierbare Namenskonventionen für Eigenschaften und Felder.

Dadurch können Namen aus der Anwendung in den vom jeweiligen Dokumentformat erwarteten Namensstil konvertiert werden.

Beispiele:

```text
PlayerName
playerName
player_name
player-name
```

Die Namenskonvertierung wird durch die gemeinsame Sasodoc-Infrastruktur durchgeführt, sodass die einzelnen Formatimplementierungen dasselbe Verhalten verwenden können.

Semantische Identifier bleiben erhalten und werden nicht wie normale Eigenschaftsnamen behandelt.

## Serialisierung

Sasodoc stellt eine Serialisierungsinfrastruktur unabhängig von den einzelnen Dokumentformaten bereit.

Serialisierungshandler können Objekte über die gemeinsamen APIs `FormatReaderBase` und `FormatWriterBase` lesen und schreiben.

Dadurch kann dieselbe Serialisierungsimplementierung mit unterschiedlichen Dokumentformaten verwendet werden.

Das Serialisierungssystem bevorzugt explizites und vorhersehbares Serialisierungsverhalten, anstatt zwingend auf automatische Laufzeit-Reflection angewiesen zu sein.

## Native AOT und Trimming

Native-AOT- und Trimming-Kompatibilität gehören zu den wichtigen Designzielen von Sasodoc.

Die Projekte aktivieren die .NET-Analyzer für Trimming und AOT, damit inkompatible APIs bereits während der Entwicklung erkannt werden können.

Sasodoc bevorzugt:

* Explizite Serialisierung
* Explizite Konverter
* Vorhersehbare Objekterzeugung
* Möglichst geringe Laufzeit-Reflection
* AOT-kompatible Dokumentverarbeitung
* Trimming-kompatible APIs

Formatimplementierungen sollten APIs vermeiden, die zur Laufzeit generierte Serialisierungsmetadaten benötigen, sofern die entsprechenden Metadaten nicht explizit bereitgestellt werden.

## Asynchrone Ein-/Ausgabe

Dokumentformate können synchrone und asynchrone Stream-Operationen bereitstellen.

```csharp
writer.SaveTo(stream);
```

Asynchron:

```csharp
await writer.SaveToAsync(stream, cancellationToken);
```

Reader stellen die entsprechenden Ladeoperationen bereit:

```csharp
reader.LoadFrom(stream);
```

sowie:

```csharp
await reader.LoadFromAsync(stream, cancellationToken);
```

Asynchrone APIs verwenden echte asynchrone Stream-Operationen, anstatt synchrone Ein-/Ausgabe lediglich mit `Task.Run` zu umschließen.

## Beispiele

Beispielprojekte befinden sich im Verzeichnis [`Examples`](Examples).

Die Beispiele zeigen die einzelnen Dokumentformate sowie wichtige Konfigurationsmöglichkeiten.

Aktuell enthalten sind:

* JSON-Grundlagen
* XML-Grundlagen
* INI-Grundlagen
* YAML-Grundlagen
* Binary-Grundlagen
* Binary ohne Identifier
* Binary mit Inline-Identifiern
* Binary mit Identifier-Registry
* Binary-Magic-Werte
* Binary-Längenkodierungen
* Asynchrone Binary-Ein-/Ausgabe
* Binary-Arrays
* JSON-Namenskonventionen

Jedes Beispiel wird als separates Projekt bereitgestellt und kann dadurch unabhängig erstellt und ausgeführt werden.

## Issues und Entwicklung

Bekannte Fehler, geplante Funktionen und die laufende Entwicklung werden über GitHub Issues verwaltet:

[GitHub Issues](https://github.com/sachssoft/Sasodoc/issues)

Bei Paketen, die als **Instabil** gekennzeichnet sind, empfiehlt es sich, die zugehörigen Issues zu prüfen, da sich deren API oder Dokumentdarstellung noch ändern kann.

Fehlerberichte und Feature-Anfragen können ebenfalls über den Issue-Tracker eingereicht werden.

## Lizenz

Sasodoc steht unter der MIT-Lizenz.

Weitere Informationen befinden sich in [LICENSE.md](LICENSE.md).
