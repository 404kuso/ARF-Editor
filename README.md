# Was ist das hier

Das hier ist ein Windows-Form Editor geschrieben in C# um das Erstellen und Bearbeiten von AnimeRoyale Komponenten zu erleichtern. 
Dazu gehören Karten- und Attackendateien.

## [Readme wurde zum Wiki verschoben](https://github.com/AnimeJunkies-TV/ARF-Editor/wiki)


<!-- 
# Inhalt

- [`Datei-Typen`](#Typen)
: Die verschiedenen Dateitypen und deren Aufbau erklärt
- [`Resourcen`](#Resourcen)
: Wichtige Werte für die Dateien
- [`Editor`](#Editor)
: Hilfe für den Editor

# Typen

ARF steht für AnimeRoyale-File und beschreibt alle Dateientypen für AnimeRoyale


## Header

Der Headerblock ist geteilt in `0x00` : `0x0D`

Das Header ist hauptsächlich immer das selbe für alle ARF-Dateien:
```
00 01 02 03 04 05 06 07 08 09 0A     0B      0C
2B 52 4D 51 49 3C 53 5D 45 50 49 {Headertyp} FF
```
wo `0x0B` (Headertyp) den Dateitypen beschreibt

**Dateitypen**
- `0x0F`: Karte
- `0x1F`: Attacke

Genau wie das Header lassen sich ARF-Dateien in verschiedene Blöcke teilen, die sich je nach ARF-Typ unterscheiden


Notiz
> Strings sind encodiert, für die Charactertabelle siehe [hier](#chartable)

## ARC

ARC (AnimeRoyale-Card) ist ein Dateiformat für eine AnimeRoyale Karte.
In dieser sind alle Informationen über die Karte gespeichert

<details>
<summary>Adressen</summary>

Hier ist die Liste von Adressen in der Karte

> Die Adressen sind alle relativ zum Block, das heißt 0x00 würde das 0te byte vom unterteilten Block sein


### Kartendetails

Allgemeine Daten über die Karte

`0x0D` : `0x160`

|		Inhalt		|		Adresse		|		 Typ		|							Beschreibung						|
|-------------------|-------------------|-------------------|---------------------------------------------------------------|
|ID					|`0x000`  :  `0x002`|ushort				|Die ID von der Karte											|
|Name				|`0x002`  :  `0x022`|string				|Der Name der Karte												|
|Beschreibung		|`0x022`  :  `0x122`|string				|Beschreibung der Karte											|
|Herkunft			|`0x122`  :  `0x142`|string				|Der Name vom Anime												|
|Zusammenspiel		|`0x142`  :  `0x14C`|ushort[5]			|Fünf IDs bei denen die Karte einen Boost im Kampf bekommt		|
|Seltenheit			|`0x14C`  :  `0x14D`|byte				|Die [Seltenheit](#seltenheit)                                  |
|Geschlecht         |`0x14D`  :  `0x14E`|byte               |Das [Geschlecht](#geschlecht)                                  |
|Elemente           |`0x14E`  :  `0x151`|byte[2]            |Die zwei [Elemente](#elemente)                                 |
|Checksum			|`0x151`  :  `0x153`|byte[2]			|Checksum zum Prüfen des Blockes								|


`0x160` : `0x162`
```
FF FF
```


### Kartenstats

Statuswerte der Karte

`0x162` : `0x16A`

|		Inhalt		|		Adresse		|		 Typ		|							Beschreibung						|
|-------------------|-------------------|-------------------|---------------------------------------------------------------|
|Level              |`0x00`   :   `0x01`|byte               |Für welches Level der Karte diese Statuswerte gelten           |
|Angriff            |`0x01`   :   `0x02`|byte               |Wie stark die Angriffe der Karte sind                          |
|Verteidigung       |`0x02`   :   `0x03`|byte               |Wie stark der erlittene Schaden verringert wird                |
|Schnelligkeit      |`0x03`   :   `0x04`|byte               |Der Wert bestimmt wer im Kampf anfängt                         |
|LP                 |`0x04`   :   `0x06`|ushort             |Die Anzahl der Lebenspunkte                                    |
|Checksum           |`0x06`   :   `0x08`|byte[2]            |Checksum zum prüfen des Blocks                                 |


`0x16A` : `0x16C`
```
FF FF
```

### Kartenattacken

Alle Attackensachen der Karte

`0x16C` : `0x176`

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


`0x174` : `0x177`
```
FF FF FF
```

#### Erlernbare Attacken

Erlernbare Attacken sind in zwei Blocke aufgeteilt:
- `0x177` : `0x1D1`
> 30 Attacken die durch Levelaufstieg erlernt werden können
- `0x1D1` : `0x235`
> 100 Attacken die durch andere Wege wie Items oder so erlernt werden können

Das Format ist gleich, der gesamte Block ist unterteilt in 130 mal [`byte`: Level, `byte, byte` (ushort) Attacken ID].
Das heißt, wenn die ersten 3 bytes des Blocks beispielsweise `05 00 02` wären, würde das heißen das bei Level 5 die Attacke mit der ID 2 erlernt werden könnte


</details>

## ARA

ARA (AnimeRoyale-Attack) ist ein Dateiformat für eine AnimeRoyale Attacke.
Hier sind Informationen über die Attacke gespeichert


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
|AnzeigeText        |`0x022`  :  `0x122`|string             |[Text](#atf) der angezeigt wird, wenn die Attacke benutzt wird |
|Typ                |`0x122`  :  `0x123`|byte               |Der [Typ](#attackentyp) der Attacke                            |
|Range				|`0x123`  :  `0x124`|byte				|Wieviele Gegner die Attacke treffen kann						|
|Effekt             |`0x124`  :  `0x125`|byte               |Der [Effekttyp](#effekttyp) von der Attacke                    |
|Effekt Stärke		|`0x125`  :  `0x126`|byte				|Wie stark der Effekt wirkt										|
|Chance             |`0x127`  :  `0x128`|byte               |Mit welcher Chance dieser Effekt eintritt                      |
|Stärke             |`0x128`  :  `0x129`|byte               |Wie stark die Attacke ist										|
|Beschreibung		|`0x128`  :  `0x228`|string				|Die Beschreibung über die Attacke								|
|Element            |`0x228`  :  `0x229`|byte               |Das [Element](#element) der Attacke                            |
|Checksum           |`0x229`  :  `0x22B`|byte[2]            |Checksum zum prüfen vom Block									|

</details>



# Resourcen

<h2 id="element">Elemente</h2>

- `0x00`: Feuer
- `0x01`: Wasser
- `0x02`: Blitz
- `0x03`: Wind
- `0x04`: Eis
- `0x05`: Pflanze
- `0x06`: Erde
- `0x07`: Gift
- `0x08`: Medizin

<h2 id="geschlecht">Geschlechter</h2>

- `0x00`: Männlich
- `0x01`: Weiblich
- `0x02`: Divers
- `0x03`: Unbekannt

<h2 id="seltenheit">Seltenheiten</h2>

- `0x00`: Gewöhnlich
- `0x01`: Ungewöhnlich
- `0x02`: Selten
- `0x03`: Episch
- `0x04`: Legender
- `0x05`: Mystisch

<h2 id="attackentyp">Attacken-Typen</h2>

- `0x00`: Angriff
- `0x01`: Verteidigung
- `0x02`: Heilung
- `0x03`: Boost
- `0x04`: Effekt

<h2 id="effekttyp">Effekt-Typen</h2>

- `0x00`: Keiner
- `0x01`: Verbrennung
- `0x02`: Vergiftung
- `0x03`: Paralyse
- `0x04`: Verwirrung
- `0x05`: Angriffswert verniedrigen
- `0x06`: Verteidigungswert verniedrigen
- `0x07`: Angriffswert und Verteidigungswert verniedrigen
- `0x08`: Schnelligkeitswert verniedrigen
- `0x09`: Angriffswert und Schnelligkeitswert verniedrigen
- `0x0A`: Angriffswert, Verteidigungswert, Schnelligkeitswert verniedrigen
- `0x0B`: Verteidigungswert, Schnelligkeitswert verniedrigen
- `0x0C`: Angriffswert erhöhen
- `0x0D`: Verteidigungswert erhöhen
- `0x0E`: Angriffswert, Verteidigungswert erhöhen
- `0x0F`: Schnelligkeitswert erhöhen
- `0x10`: Angriffswert und Schnelligkeitswert erhöhen
- `0x11`: Angriffswert, Verteidigungswert, Schnelligkeitswert erhöhen
- `0x12`: Verteidigungswert, Schnelligkeitswert erhöhen

<h2 id="atf">Anzeige-Text Format</h2>

- `{executor}`: Die Karte die die Attacke ausführt
- `{target}`: Die Karte auf die die Attacke angewendet werden soll
- `{attack}`: Der Name der Attacke


<h2 id="chartable">Chartable</h2>

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
|`\n`  |0x6A		|
|` `   |0x70        |
|`,`   |0x71        |
|`.`   |0x72        |
|`{`   |0x80		|
|`}`   |0x81		|
|`-`   |0x82        |
|`(`   |0x83        |
|`)`   |0x84        |

## Checksum

Die Checksum wird berrechnet, indem alles vom Block außer die letzten 2 bytes (die Checksum)

Python Code:
```py
def CRC16_CCITT(data: bytearray, start: int = 0, length: int = None):
    top = 0xFF
    bot = 0xFF

    end = start + (length or len(data))
    for i in range(start, end):
        x = data[i] ^ top
        x ^= (x >> 4)

        top = (bot ^ (x >> 3) ^ (x << 4)) % 256
        bot = (x ^ (x << 5)) % 256

    return (top << 8 | bot)
```

C# Code:
```py
public static ushort CRC16_CCITT(byte[] data, int start, int lenght)
{
    byte top = 0xFF;
    byte bot = 0xFF;

    int end = start + lenght;
    for(int i = 0; i < end; i++)
    {
        var x = data[i] ^ top;
        x ^= (x >> 4);
        
        top = (byte)(bot ^ (x >> 3) ^ (x << 4));
        bot = (byte)(x ^ (x << 5));
    }
    return (ushort)(top << 8 | bot);
}
```

# Editor

Es gibt zwei Editoren, den ARC und ARA Editor.

## Regeln

Es gibt spezielle "Regeln" im Editor. Wenn beispielsweise der Inhalt vom Textfeld zu lang ist, wird jede weitere Eingabe ignoriert

### Textfelder

- Zu langer Text
: Wenn der Text zu lang ist, wird weitere Eingabe ignoriert
- Kein gültiger Character (siehe [Chartable](#chartable))
: Wenn ein ungültiges Zeichen eingegeben wurde, wird dieses ignoriert

### Auswahlmenüs

- Nichts ausgewählt
: Wenn nichts richtig ausgewählt wurde, wird beim Verlassen des Felds automatisch das erste Element in der Auswahlliste ausgewählt

### Zahlenfelder

- Zahl zu hoch oder zu niedrig
: Wenn eine Zahl zu hoch oder zu niedrig ist, wird es automatisch zum Maximum/Minimum gesetzt

## Menu

-	<details>
	<summary>Datei</summary>

	-	<details>
		<summary>Neu</summary>

		- `Karte`
		: Erstellt eine neue Karte
		- `Attacke`
		: Erstellt eine neue Attacke

		</details>

	- `öffnen`
	: Öffnet eine Datei und den dazugehörigen Editor

	- `Speichern`
	: Überschreibt die aktuell geöffnete Datei wenn eine geöffnet wurde, ansonsten wird der _speichern unter_ Dialog angezeigt

	- `Speichern unter`
	: Speichert die aktuelle Daten in einer neuen Datei


	</details>
-	<details>
	<summary>Tools</summary>

	- `In Datenbank eintragen`
	: Speichert die aktuellen Daten in der Datenbank (sollte nicht notwendig sein zu nutzen)

	- `Prüfen`
	: Sieht je nach Editor anders aus. Hiermit können die aktuellen Daten auf Gültigkeit überprüft werden

	-	<details>
		<summary>Fenster</summary>

		-	<details>
			<summary>Wechseln</summary>
			
			- `ARC-Editor`
			: Wechselt zum Karteneditor
			- `ARA-Editor`
			: Wechselt zum Attackeneditor

			</details>

		</details>

	</details>

-   <details>
    <summary>Optionen</summary>

    - `Datenbankpfad auswählen`
    : Setzt den Pfad zur Datenbank für Vervollständigung von Attacken und Karten

    </details>

</details>

## ARC-Editor

### Allgemein

> Hier sind allgemeine Informationen über die Karte

- `ID`
: Die ID von der Karte

- `Name`
: Der Name von der Karte

- `Beschreibung`
: Eine kurze Beschreibung von der Karte

- `Herkunft`
: Der Anime aus der die Karte stammt

- `Seltenheit`
: Wie selten die Karte ist

- `Geschlecht`
: Das Geschlecht der Karte

- `Boost mit Karten`
: Fünf Karten, bei denen diese Karte im Kampf einen Statuswertboost bekommt


### Stats

> Hier sind alle Statuswerte der Karte

- `Level`
: Das Level der Karte wenn man diese erhält

- `Statuswerte`
: Dieses Feld zeigt die Summe der Statuswerte an, wenn dieses Feld überschritten wird, sind die Werte ungültig

- `Angriff`
: Wie stark die Angriffe der Karte sein sollen

- `Verteidigung`
: Wie stark der gegnerische Schaden verringert werden soll

- `Schnelligkeit`
: Dieser Wert gibt an welche Karte zuerst ran kommt, außerdem wird bei höherer Schnelligkeit die Chance auf einen Volltreffer verringert

- `Lebenspunkte`
: Wieviele Lebenspunkte die Karte hat

- `Attacken`
: 4 Attacken die die Karte standartmäßig für den Kampf besitzt

- `Erlernbare Attacken`
: 30 Attacken die per Levelaufstieg erlernt werden können, `Erlernbare Attacke` ist die Attacke die erlernt werden kann und `Level` das Level das erreicht werden muss um diese Attacke zu erlernen

### Attacken erlernbar per Item

Eine Liste von Attacken, die durch andere Wege wie Levelaufstieg erlernt werden können

## ARA-Editor

- `ID`
: Die ID von der Attacke

- `Name`
: Der Name der Attacke

- `Anzeigetext`
: Der Text der Angezeigt wird wenn die Attacke ausgeführt wird. Siehe [Formatierung](#atf)

- `Typ`
: Der Typ der Attacke
    - `Angriff`
    : Die Attacke macht Schaden

    - `Verteidigung`
    : Die Attacke verringert Schaden

    - `Heilung`
    : Die Attacke heilt Schaden

    - `Boost`
    : Die Attacke boostet Statuswerte einer eigenen Karte

    - `Statusänderung`
    : Die Attacke gibt einer gegnerischen Karte einen schlechten Effekt

- `Stärke`
: Wie stark die Attacke ist

- `Fläche`
: Wieviele Gegner die Attacke trifft.
> Bei 2 Gegnern wird das ausgewählte Ziel und ein Zufälliges der restlichen zwei Karten getroffen, bei 3 werden alle gegnerischen Karten getroffen

- [x] `Statusänderung bei Attacke`
: Wenn dieses Feld ausgewählt wird, macht die Attacke zusätlich einen Effekt

- `Status`
: Der Effekt der das Ziel erhält

- `Chance`
: Wie die Chance steht das dieser Effekt eintritt (1 zu dem Wert)

- `Beschreibung`
: Die Beschreibung von der Attacke -->
