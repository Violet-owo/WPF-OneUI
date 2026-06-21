# Samsung One UI for WPF

> [!NOTE]
> 🇬🇧 A beautiful, modern, and fluid UI component library for WPF, inspired by Samsung One UI design guidelines.
> 🇮🇹 Una libreria di componenti UI bella, moderna e fluida per WPF, ispirata alle linee guida del design Samsung One UI.

[![WPF](https://img.shields.io/badge/WPF-.NET_10-purple.svg)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Author](https://img.shields.io/badge/Author-Violet%20Miller-blue.svg)](#)

---

## 🇬🇧 English

### ✨ Features
- **Fluid Animations**: Smooth transitions and micro-interactions typical of One UI.
- **Dynamic Theming**: Easily switch between Light and Dark mode using the built-in `ThemeManager`.
- **Rich Components**: Cards, Buttons, Calendar with dot-indicators, Animated Bar Charts, and more.
- **Modular Input Fields & Modals**: Highly customizable inputs and animated overlay modals for data insertion.

### 📦 Installation
*(Soon available on NuGet)*
For now, clone this repository and add a `ProjectReference` to the `SamsungUi` library in your solution.

### 🚀 Usage
Open your `App.xaml` and include the unified control dictionary along with your preferred starting color scheme:

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="pack://application:,,,/SamsungUi;component/Themes/ColorsLight.xaml"/>
            <ResourceDictionary Source="pack://application:,,,/SamsungUi;component/Themes/SamsungUi.Controls.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

You can easily switch the application theme at runtime:
```csharp
using SamsungUi.Appearance;
ThemeManager.ApplyTheme(ThemeType.Dark); // or ThemeType.Light
```

---

## 🇮🇹 Italiano

### ✨ Funzionalità
- **Animazioni Fluide**: Transizioni morbide e micro-interazioni tipiche di One UI.
- **Temi Dinamici**: Passa facilmente dalla modalità Chiara a Scura usando il `ThemeManager` integrato.
- **Componenti Ricchi**: Card, Pulsanti, Calendario con indicatori, Grafici a barre animati e altro ancora.
- **Campi di Input e Modali Modulari**: Input altamente personalizzabili e modali sovrapposti animati per l'inserimento dati.

### 📦 Installazione
*(Presto disponibile su NuGet)*
Per ora, clona questa repository e aggiungi una `ProjectReference` alla libreria `SamsungUi` nella tua soluzione.

### 🚀 Utilizzo
Apri il file `App.xaml` e includi il dizionario dei controlli unificato insieme alla combinazione di colori preferita per la partenza:

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="pack://application:,,,/SamsungUi;component/Themes/ColorsLight.xaml"/>
            <ResourceDictionary Source="pack://application:,,,/SamsungUi;component/Themes/SamsungUi.Controls.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

Puoi cambiare il tema dell'applicazione a runtime:
```csharp
using SamsungUi.Appearance;
ThemeManager.ApplyTheme(ThemeType.Dark); // o ThemeType.Light
```

---
**Samsung One UI for WPF** - Created by Violet Miller.
