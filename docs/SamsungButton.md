# SamsungButton

Il `SamsungButton` è il componente principale per le azioni dell'utente. È stato progettato per ricalcare i classici pulsanti della One UI, caratterizzati da angoli fortemente arrotondati, uno stile "pillola" e un feedback visivo immediato al passaggio del mouse e al click.

![SamsungButton Example](images/samsungbutton_preview.png)
*(Sostituisci questa immagine inserendo uno screenshot in `docs/images/samsungbutton_preview.png`)*

---

## Ereditarietà
Questo controllo eredita direttamente dalla classe nativa WPF `System.Windows.Controls.Button`.
Supporta nativamente tutti gli eventi e i binding classici (`Click`, `Command`, `CommandParameter`, ecc.).

---

## Proprietà Personalizzate (Dependency Properties)

| Proprietà | Tipo | Valore di Default | Descrizione |
|-----------|------|-------------------|-------------|
| **Variant** | `ButtonVariant` | `Normal` | Definisce lo stile del pulsante. Può essere impostato su `Normal` (sfondo grigio chiaro/scuro in base al tema) oppure `Primary` (utilizza il colore di accento principale). |
| **CornerRadius** | `CornerRadius` | `20` | Smussatura degli angoli. Di default è impostato a 20 per ottenere il tipico effetto a "pillola". |

### Enumerazione `ButtonVariant`
- `Normal`: Sfondo neutro, ideale per pulsanti secondari o azioni standard (Annulla, Indietro, ecc.).
- `Primary`: Sfondo colorato e testo bianco, perfetto per la *Call To Action* principale (Salva, Invia, Conferma).

---

## Come Usarlo

Ecco alcuni esempi pratici da inserire nel tuo file XAML:

### 1. Pulsante Standard (Normal)
Il pulsante di base. Utilizza i colori di superficie della One UI.

```xml
<sui:SamsungButton Content="Pulsante Secondario" 
                   Click="MioBottone_Click" />
```

### 2. Pulsante Primario (Primary)
Ideale per guidare l'attenzione dell'utente verso l'azione più importante della pagina.

```xml
<sui:SamsungButton Content="Pulsante Principale" 
                   Variant="Primary" 
                   Width="200" />
```

### 3. Pulsante Disabilitato
Il design gestisce automaticamente lo stato `IsEnabled="False"`, riducendo l'opacità per far capire all'utente che l'azione non è disponibile.

```xml
<sui:SamsungButton Content="Non Cliccabile" 
                   IsEnabled="False" />
```

---

## Comportamento Visivo (Stati)
Il template del controllo gestisce automaticamente le transizioni di colore e le animazioni:
- **Hover (`IsMouseOver`)**: Il colore di sfondo cambia dolcemente usando la risorsa `OneUiSurfaceBrush` o schiarendo il colore primario.
- **Pressed (`IsPressed`)**: Viene applicato un effetto di oscuramento/rimpicciolimento per restituire una sensazione tattile realistica.
- **Disabled (`IsEnabled=False`)**: Abbassa l'opacità globale del componente al 50%.
