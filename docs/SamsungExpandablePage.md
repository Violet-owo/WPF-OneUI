# SamsungExpandablePage

Il `SamsungExpandablePage` ricrea la tipica struttura gerarchica dell'app Impostazioni di Samsung: un'intestazione gigantesca (Hero Title) che si rimpicciolisce elegantemente diventando un titolo fisso in alto (Sticky Header) quando l'utente scorre la pagina.

![SamsungExpandablePage Example](../Screen/SamsungExpandablePage.png)
> 📸 *Lo screenshot è in pausa caffè! Lo sviluppatore lo caricherà a breve.*

---

## 🇬🇧 English

The `SamsungExpandablePage` recreates the typical hierarchical layout of the Samsung Settings app: a massive hero title that elegantly shrinks and snaps to the top as a sticky header when the user scrolls down the page.

### Inheritance
Inherits from `System.Windows.Controls.ContentControl`. You place your scrolling items directly inside its `Content`.

### Custom Properties

| Property | Type | Default Value | Description |
|-----------|------|-------------------|-------------|
| **PageTitle** | `string` | `"Page Title"` | The title text that appears huge at the top, and small when scrolled. |

### Visual Behavior
- **Scroll Synchronization**: Binds an internal `ScrollViewer`'s `VerticalOffset` to the scale and opacity of the massive title, simulating a parallax contraction.
- **Sticky Header**: Once scrolled past a threshold, the huge title fades out, and a smaller, elegant title fades in at the very top, remaining fixed (sticky).

### How to Use
```xml
<sui:SamsungExpandablePage PageTitle="Settings">
    <StackPanel Margin="24">
        <!-- Add cards and settings here. A ScrollViewer is built-in. -->
        <sui:SamsungCard Height="800" />
    </StackPanel>
</sui:SamsungExpandablePage>
```

---

## 🇮🇹 Italiano

Il `SamsungExpandablePage` ricrea la tipica struttura gerarchica dell'app Impostazioni di Samsung: un'intestazione gigantesca che si rimpicciolisce elegantemente diventando un titolo fisso in alto (Sticky Header) non appena l'utente inizia a scorrere la pagina.

### Ereditarietà
Eredita da `System.Windows.Controls.ContentControl`. Puoi inserire la tua intera pagina direttamente all'interno del suo `Content`.

### Proprietà Personalizzate

| Proprietà | Tipo | Valore di Default | Descrizione |
|-----------|------|-------------------|-------------|
| **PageTitle** | `string` | `"Page Title"` | Il testo del titolo che appare gigantesco in cima e rimpicciolito nella barra di navigazione fissa. |

### Comportamento Visivo
- **Sincronizzazione Scorrimento**: Legge in tempo reale l'offset (lo scorrimento) dello `ScrollViewer` interno per calcolare dinamicamente la dimensione e l'opacità del titolo grande.
- **Intestazione Fissa (Sticky)**: Superata una certa soglia di scroll, il titolo grande sparisce del tutto e appare in dissolvenza un titolo più piccolo ed elegante ancorato in cima alla finestra.

### Come Usarlo
```xml
<sui:SamsungExpandablePage PageTitle="Impostazioni">
    <StackPanel Margin="24">
        <!-- Inserisci qui il contenuto. Lo ScrollViewer è già incluso! -->
        <sui:SamsungCard Height="800" />
    </StackPanel>
</sui:SamsungExpandablePage>
```
