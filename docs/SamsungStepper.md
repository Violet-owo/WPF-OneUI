# SamsungStepper

Il `SamsungStepper` (o Wizard) è fondamentale nei gestionali per mostrare all'utente l'avanzamento all'interno di un processo a più fasi (es. checkout, creazione guidata).

![SamsungStepper Example](../Screen/SamsungStepper.png)
> 📸 *Lo screenshot è in pausa caffè! Lo sviluppatore lo caricherà a breve.*

---

## 🇬🇧 English

The `SamsungStepper` (or Wizard timeline) is fundamental in enterprise software for showing the user's progress through a multi-step process (e.g., checkout, guided creation).

### Inheritance
- `SamsungStepper` inherits from `System.Windows.Controls.ItemsControl`.
- `SamsungStepperItem` inherits from `System.Windows.Controls.ContentControl`.

### Custom Properties

**In `SamsungStepper`:**
| Property | Type | Default Value | Description |
|-----------|------|-------------------|-------------|
| **Orientation** | `Orientation` | `Horizontal` | Direction of the steps. Can be `Horizontal` or `Vertical`. |

**In `SamsungStepperItem`:**
| Property | Type | Default Value | Description |
|-----------|------|-------------------|-------------|
| **StepIndex** | `string` | `"1"` | The number or text to show inside the circle. |
| **IsCurrent** | `bool` | `False` | Highlights the step with a solid border and primary text, indicating the active stage. |
| **IsCompleted** | `bool` | `False` | Replaces the number with a checkmark icon and fills the circle. |
| **IsLastStep** | `bool` | `False` | Hides the connecting line after this item. |

### Visual Behavior
- **Connecting Lines**: Automatically draws gray lines between items. The line stops rendering on the item marked with `IsLastStep="True"`.
- **States**: Transitions beautifully between pending (gray), current (outlined primary), and completed (filled primary with checkmark).

### How to Use
```xml
<sui:SamsungStepper Orientation="Horizontal">
    <sui:SamsungStepperItem StepIndex="1" IsCompleted="True">
        <TextBlock Text="Account" />
    </sui:SamsungStepperItem>
    <sui:SamsungStepperItem StepIndex="2" IsCurrent="True">
        <TextBlock Text="Payment" />
    </sui:SamsungStepperItem>
    <sui:SamsungStepperItem StepIndex="3" IsLastStep="True">
        <TextBlock Text="Confirm" />
    </sui:SamsungStepperItem>
</sui:SamsungStepper>
```

---

## 🇮🇹 Italiano

Il `SamsungStepper` (o Wizard) è fondamentale nei gestionali per mostrare all'utente l'avanzamento all'interno di un processo a più fasi (es. checkout, creazione guidata).

### Ereditarietà
- `SamsungStepper` eredita da `System.Windows.Controls.ItemsControl`.
- `SamsungStepperItem` eredita da `System.Windows.Controls.ContentControl`.

### Proprietà Personalizzate

**In `SamsungStepper`:**
| Proprietà | Tipo | Valore di Default | Descrizione |
|-----------|------|-------------------|-------------|
| **Orientation** | `Orientation` | `Horizontal` | Direzione della linea temporale. Può essere `Horizontal` o `Vertical`. |

**In `SamsungStepperItem`:**
| Proprietà | Tipo | Valore di Default | Descrizione |
|-----------|------|-------------------|-------------|
| **StepIndex** | `string` | `"1"` | Il numero o il testo da mostrare dentro il cerchio del passaggio. |
| **IsCurrent** | `bool` | `False` | Evidenzia il passaggio con un bordo spesso e testo colorato per indicare la fase attiva. |
| **IsCompleted** | `bool` | `False` | Sostituisce il numero con l'icona di una spunta e colora tutto il cerchio a tinta unita. |
| **IsLastStep** | `bool` | `False` | Nasconde la linea grigia di connessione alla destra di questo elemento. |

### Comportamento Visivo
- **Linee di Connessione**: Disegna automaticamente linee di collegamento tra gli step. La linea si interrompe sull'elemento con `IsLastStep="True"`.
- **Stati**: Gestisce 3 stadi visivi chiarissimi: Da fare (grigio), In Corso (Bordo colorato), Completato (Sfondo colorato con spunta bianca).

### Come Usarlo
```xml
<sui:SamsungStepper Orientation="Horizontal">
    <sui:SamsungStepperItem StepIndex="1" IsCompleted="True">
        <TextBlock Text="Account" />
    </sui:SamsungStepperItem>
    <sui:SamsungStepperItem StepIndex="2" IsCurrent="True">
        <TextBlock Text="Pagamento" />
    </sui:SamsungStepperItem>
    <sui:SamsungStepperItem StepIndex="3" IsLastStep="True">
        <TextBlock Text="Conferma" />
    </sui:SamsungStepperItem>
</sui:SamsungStepper>
```
