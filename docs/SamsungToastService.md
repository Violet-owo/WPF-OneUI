# SamsungToastService

### Screenshots
| Light Mode | Dark Mode |
|:---:|:---:|
| ![Light](../Screen/SamsungNotification.png) | ![Dark](../Screen/SamsungNotification.png) |


Il `SamsungToastService` non è un elemento XAML ma un **servizio C#**. Permette di mostrare piccoli messaggi di notifica fluttuanti (i "Toast" tipici di Android) richiamabili da codice senza inquinare l'interfaccia grafica.

![SamsungToastService Example](../Screen/SamsungToastService.png)
> 📸 *Lo screenshot è in pausa caffè! Lo sviluppatore lo caricherà a breve.*

---

## 🇬🇧 English

The `SamsungToastService` is not a XAML element but a **C# Service**. It allows you to fire off small, floating notification messages (Android-style "Toasts") programmatically, without cluttering your UI code.

### Usage (C# API)
The service creates a `SamsungNotificationCard` dynamically on a transparent full-screen window layer.

**Method Signature:**
```csharp
public static void Show(string title, string message, Action onClick = null, bool isError = false)
```

| Parameter | Type | Description |
|-----------|------|-------------|
| **title** | `string` | The bold header text. |
| **message** | `string` | The secondary detail text. |
| **onClick** | `Action` | (Optional) Code to run if the user clicks the toast. |
| **isError** | `bool` | (Optional) If true, the toast is styled with the Danger/Red color. |

### Visual Behavior
- **Slide-in**: The toast slides up fluidly from the bottom-center of the active window.
- **Auto-dismiss**: After 3-4 seconds, it automatically slides back down and destroys itself.
- **Stacking**: Not natively designed for infinite stacking, but multiple calls will rapidly replace/overwrite cleanly.

### How to Use (C#)
```csharp
// Simple Info Toast
SamsungToastService.Show("Download Complete", "The file was saved.");

// Error Toast
SamsungToastService.Show("Error", "Could not connect to database.", isError: true);
```

---

## 🇮🇹 Italiano

Il `SamsungToastService` non è un controllo XAML, ma un vero e proprio **servizio C# statale**. Ti consente di far apparire piccoli messaggi a comparsa (i famosi "Toast" in stile Android) richiamandoli via codice da qualsiasi punto del programma, senza dover inserire nulla nello XAML!

### Utilizzo (C# API)
Il servizio genera a runtime una `SamsungNotificationCard` all'interno di una finestra trasparente sovrapposta, calcolando la posizione perfetta in base all'applicazione attiva.

**Firma del Metodo:**
```csharp
public static void Show(string title, string message, Action onClick = null, bool isError = false)
```

| Parametro | Tipo | Descrizione |
|-----------|------|-------------|
| **title** | `string` | Il titolo principale in grassetto. |
| **message** | `string` | Il messaggio descrittivo. |
| **onClick** | `Action` | (Opzionale) Una funzione/Lambda da eseguire se l'utente clicca sul fumetto. |
| **isError** | `bool` | (Opzionale) Se True, il fumetto si colora di rosso (Danger) per segnalare un errore critico. |

### Comportamento Visivo
- **Slide-in (Ingresso)**: Il Toast emerge fluidamente dal basso (in centro alla finestra).
- **Auto-chiusura**: Dopo circa 3 secondi, il Toast scivola verso il basso e si distrugge autonomamente liberando memoria.
- **Rilievo Visivo**: Ha una forte ombreggiatura e bordi totalmente arrotondati, spiccando molto bene rispetto al contenuto sottostante.

### Come Usarlo (C#)
Basta richiamare il metodo statico ovunque tu sia nel tuo codice:

```csharp
// Toast Informativo base
SamsungToastService.Show("Download Completato", "Il file è stato salvato con successo.");

// Toast di Errore
SamsungToastService.Show("Errore di Sistema", "Impossibile connettersi al database.", isError: true);
```

