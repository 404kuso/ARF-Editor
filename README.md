# Was ist das hier

Das hier ist ein Windows-Form Editor geschrieben in C# um das Erstellen und Bearbeiten von AnimeRoyale Komponenten zu erleichtern. 
Dazu gehören Karten- und Attackendateien.

# Typen

ARF steht für AnimeRoyale-File und beschreibt alle Dateientypen für AnimeRoyale


## Header

Der Headerblock ist geteilt in `0x00` : `0x0D`

Das Header ist hauptsächlich immer das selbe für alle ARF-Dateien:
```
2B 52 4D 51 49 3C 53 5D 45 50 49 {Headertyp} FF
```
wo `0x0D` (Headertyp) den Dateitypen beschreibt (siehe unten Dateitypen)

Genau wie das Header lassen sich ARF-Dateien in verschiedene Blöcke teilen, die sich je nach ARF-Typ unterscheiden



Notiz
> Strings sind encodiert, für die Charactertabelle siehe [hier](#Chartable)

## ARC

ARC (AnimeRoyale-Card) ist ein Dateiformat für eine AnimeRoyale Karte.
In dieser sind alle Informationen über die Karte gespeichert

Der Headertyp für eine Karte ist `0x0F`

<details>
<summary>Adressen</summary>

Hier ist die Liste von Adressen in der Karte

> Die Adressen sind alle relativ zum Block, das heißt 0x00 würde das 0te byte vom unterteilten Block sein


### Kartendetails

Allgemeine Daten über die Karte

`0x0D` : `0x15D`

|		Inhalt		|		Adresse		|		 Typ		|							Beschreibung						|
|-------------------|-------------------|-------------------|---------------------------------------------------------------|
|ID					|`0x000`  :  `0x002`|ushort				|Die ID von der Karte											|
|Name				|`0x002`  :  `0x022`|string				|Der Name der Karte												|
|Beschreibung		|`0x022`  :  `0x122`|string				|Beschreibung der Karte											|
|Herkunft			|`0x122`  :  `0x142`|string				|Der Name vom Anime												|
|Zusammenspiel		|`0x142`  :  `0x14C`|ushort[5]			|Fünf IDs bei denen die Karte einen Boost im Kampf bekommt		|
|Seltenheit			|`0x14C`  :  `0x14D`|byte				|Das Geschlecht (0 männlich, 1 webilich, 2 divers, 3 unbekannt) |
|Checksum			|`0x14E`  :  `0x150`|byte[2]			|Checksum zum prüfen des Blockes								|


`0x15D` : `0x160`
```
FF FF FF
```


### Kartenstats

Statuswerte der Karte

`0x160` : `0x168`

|		Inhalt		|		Adresse		|		 Typ		|							Beschreibung						|
|-------------------|-------------------|-------------------|---------------------------------------------------------------|
|Level              |`0x00`   :   `0x01`|byte               |Für welches Level der Karte diese Statuswerte gelten           |
|Angriff            |`0x01`   :   `0x02`|byte               |Wie stark die Angriffe der Karte sind                          |
|Verteidigung       |`0x02`   :   `0x03`|byte               |Wie stark der erlittene Schaden verringert wird                |
|Schnelligkeit      |`0x03`   :   `0x04`|byte               |Der Wert bestimmt wer im Kampf anfängt                         |
|LP                 |`0x04`   :   `0x06`|ushort             |Die Anzahl der Lebenspunkte                                    |
|Checksum           |`0x06`   :   `0x08`|byte[2]            |Checksum zum prüfen des Blocks                                 |


`0x168` : `0x16A`
```
FF FF
```

### Kartenattacken

Alle Attackensachen der Karte

`0x16A` : `0x174`

> Der Attackenblock wird nochmal in 3 verschiedene Blöche unterteilt

#### Attacken

Die Attacken die die Karte im Kampf besitzt

`0x16A` : `0x174`

|		Inhalt		|		Adresse		|		 Typ		|							Beschreibung						|
|-------------------|-------------------|-------------------|---------------------------------------------------------------|
|1. Attacken ID     |`0x00`   :   `0x02`|ushort             |Die ID von der ersten Attacke                                  |
|2. Attacken ID     |`0x02`   :   `0x04`|ushort             |Die ID von der zweiten Attacke                                 |
|3. Attacken ID     |`0x04`   :   `0x06`|ushort             |Die ID von der dritten Attacke                                 |
|4. Attacken ID     |`0x06`   :   `0x08`|ushort             |Die ID von der vierten Attacke                                 |
|Checksum           |`0x08`   :   `0x0A`|byte[2]            |Checksum zum prüfen vom Block                                  |


#### Erlernbare Attacken

Erlernbare Attacken sind in zwei Blocke aufgeteilt:
- `0x177` : `0x1D1`
> 30 Attacken die durch Levelaufstieg erlernt werden können
- `0x1D1` : `0x235`
> 100 Attacken die durch andere Wege wie Items oder so erlernt werden können

Das Format ist gleich, der Block ist unterteilt in jeweils 115 mal [`byte`: Level, `byte, byte` (ushort) Attacken ID].
Das heißt, wenn die ersten 3 bytes des Blocks beispielsweise `05 00 02` wären, würde das heißen das bei Level 5 die Attacke mit der ID 2 erlernt werden könnte


</details>

## ARA

ARA (AnimeRoyale-Attack) ist ein Dateiformat für eine AnimeRoyale Attacke.
Hier sind Informationen über die Attacke gespeichert

Der Headertyp für eine Attacke ist `0x1F`

<details>
<summary>Adressen</summary>

Hier ist die Liste von Adressen in der Karte

> Die Adressen sind alle relativ zum Block, das heißt 0x00 würde das 0te byte vom unterteilten Block sein

### Body

Die Attacke ist nur in zwei Blöcke geteilt, das Header und der Body

`0x0D` : `0x135`

|		Inhalt		|		Adresse		|		 Typ		|							Beschreibung						|
|-------------------|-------------------|-------------------|---------------------------------------------------------------|
|ID                 |`0x000`  :  `0x002`|ushort             |Die ID der Attacke                                             |
|Name               |`0x002`  :  `0x022`|string             |Der Name der Karte                                             |
|AnzeigeText        |`0x022`  :  `0x122`|string             |Der Text der angezeigt wird wenn die Attacke ausgeführt wird   |
|Typ                |`0x122`  :  `0x123`|byte               |Der Typ der Attacke                                            |
|Effekt             |`0x123`  :  `0x124`|byte               |Der Effekttyp von der Attacke                                  |
|Chance             |`0x124`  :  `0x125`|byte               |Mit welcher Chance dieser Effekt eintritt                      |
|Stärke             |`0x125`  :  `0x126`|byte               |Wie stark die Attacke ist
|Checksum           |`0x126`  :  `0x128`|byte[2]            |Checksum zum prüfen vom Block

**Attacken-Typen**
- `0x00`: Angriff
- `0x01`: Verteidigung
- `0x02`: Heilung
- `0x03`: Boost
- `0x04`: Effekt

**Effekt-Typen**
- `0x00`: Keiner
- `0x01`: Verbrennung
- `0x02`: Vergiftung
- `0x03`: Paralyse
- `0x04`: Verwirrung
- `0x05`: Angriffswert verniedrigen
- `0x06`: Verteidigungswert verniedrigen
- `0x07`: Angriffswert und Verteidigungswert verniedrigen

</details>


# Chartable

Die Charactertabelle zum encodieren/decodieren von Text

| Char |  Encodiert |
|------|------------|
|`0`   |0x21        |
|`1`   |0x22        |
|`2`   |0x23        |
|`3`   |0x24        |
|`4`   |0x25        |
|`5`   |0x26        |
|`6`   |0x27        |
|`7`   |0x28        |
|`8`   |0x29        |
|`9`   |0x2A        |
|`A`   |0x2B        |
|`B`   |0x2C        |
|`C`   |0x2D        |
|`D`   |0x2E        |
|`E`   |0x2F        |
|`F`   |0x30        |
|`G`   |0x31        |
|`H`   |0x32        |
|`I`   |0x33        |
|`J`   |0x34        |
|`K`   |0x35        |
|`L`   |0x36        |
|`M`   |0x37        |
|`N`   |0x38        |
|`O`   |0x39        |
|`P`   |0x3A        |
|`Q`   |0x3B        |
|`R`   |0x3C        |
|`S`   |0x3D        |
|`T`   |0x3E        |
|`U`   |0x3F        |
|`V`   |0x40        |
|`W`   |0x41        |
|`X`   |0x42        |
|`Y`   |0x43        |
|`Z`   |0x44        |
|`a`   |0x45        |
|`b`   |0x46        |
|`c`   |0x47        |
|`d`   |0x48        |
|`e`   |0x49        |
|`f`   |0x4A        |
|`g`   |0x4B        |
|`h`   |0x4C        |
|`i`   |0x4D        |
|`j`   |0x4E        |
|`k`   |0x4F        |
|`l`   |0x50        |
|`m`   |0x51        |
|`n`   |0x52        |
|`o`   |0x53        |
|`p`   |0x54        |
|`q`   |0x55        |
|`r`   |0x56        |
|`s`   |0x57        |
|`t`   |0x58        |
|`u`   |0x59        |
|`v`   |0x5A        |
|`w`   |0x5B        |
|`x`   |0x5C        |
|`y`   |0x5D        |
|`z`   |0x5E        |
|`ß`   |0x5F        |
|`ä`   |0x60        |
|`ö`   |0x61        |
|`ü`   |0x62        |
|`Ä`   |0x63        |
|`Ö`   |0x64        |
|`Ü`   |0x65        |
|` `   |0x70        |
|`,`   |0x71        |
|`.`   |0x72        |
