# Sachssoft Sasodoc

**Sasodoc** ist ein leichtgewichtiges Entwickler-Tool zur Unterstützung bei der Serialisierung zwischen strukturierten Datenformaten und Dokumentmodellen (Format ⇄ Daten).

Ziel ist es, den Aufwand für Entwickler bei der Verarbeitung und Abbildung strukturierter Daten deutlich zu reduzieren.

---

## ✨ Aktueller Stand

Sasodoc befindet sich in einer **ersten Veröffentlichung**.

* Der Fokus liegt derzeit auf **JSON**
* Weitere Formate sind langfristig geplant

---

## ⚠️ Wichtiger Hinweis zur Serialisierung

Im Gegensatz zu klassischen Serializern unterstützt Sasodoc aktuell **keine vollständige, rekursive Serialisierung kompletter Objektstrukturen**.

Diese Funktion ist derzeit **noch nicht implementiert**, kann aber in zukünftigen Versionen ergänzt werden.

Stattdessen arbeitet Sasodoc momentan mit einem **vereinfachten, komponentenbasierten Ansatz**, um einzelne Teile von Datenstrukturen zu verarbeiten.

---

## 🧠 Zielsetzung

Sasodoc soll Entwicklern helfen:

* strukturierte Daten einfacher zu verarbeiten
* Mapping zwischen Daten und Dokumentstrukturen zu vereinfachen
* wiederkehrenden Serialisierungsaufwand zu reduzieren
* schneller mit Konfigurations- und Datendateien zu arbeiten

---

## 📦 Unterstütze Formate

| Format | Status         | Minimum .NET Version | NuGet |
| ------ | -------------- | -------------------- | ----- |
| Basis   | ✅ Verfügbar    | 5.0				 | [![NuGet](https://img.shields.io/nuget/v/Sachssoft.Sasodoc)](https://www.nuget.org/packages/Sachssoft.Sasodoc)      |
| JSON   | ✅ Verfügbar    | 7.0				 | [![NuGet](https://img.shields.io/nuget/v/Sachssoft.Sasodoc)](https://www.nuget.org/packages/Sachssoft.Sasodoc)      |
| XML    | 🚧 In Planung |						 | |
| YAML   | 🚧 In Planung |						 |       |
| TOML   | 🚧 In Planung |						 |       |

---

## 🚀 Perspektive

Geplant sind unter anderem:

* Erweiterung der Formatunterstützung (XML, YAML, TOML)
* Verbesserte Serialisierungsfunktionen
* Mögliche vollständige Objektverarbeitung
* Weitere Vereinfachungen für Entwickler-Workflows

---

## 🤝 Contributing

Beiträge, Ideen und Feedback sind willkommen.
