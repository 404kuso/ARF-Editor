# Header (0 - 0x0D)

- {0x00 - 0x0B}
    General Header [11]
        `2B 52 4D 51 49 3C 53 5D 45 50 49`
- {0x0B - 0x0C}
    Typ [1]
        0F = Karte, 1F = Attacke, 2F = Item
- {0x0C - 0x0D}
    Ende [1]
        0xFF


# Kartendetails [150] (0x0D - 0x15D)

- {0x00 - 0x02}
    ID[2] (0x0C-0x0E)
        Die ID von der Karte
- {0x02 - 0x22}
    Name [32] (0x0E - 0x2E)
        Der Name der Karte
- {0x22 - 0x122}
    Beschreibung [256] (0x2E - 0x12E)
        Die Beschreibung von der Karte
- {0x122-0x142}
    Herkunft [32] (0x12E - 0x14F)
        Aus welchem Anime die Karte stammt
- {0x142 - 0x14C} 
    ZusammenSpiel [10] (0x14F - 0x159)
        Mit welchen KartenIDs (2 bytes, max 5 KartenIDs), es einen Boost gibt
- {0x14C - 0x14D}
    Seltenheit[1] (0x159 - 0x15A)
        Welche Seltenheit die Karte hat (0 == gewöhnlich, 3 == episch)
- {0x14D -  0x14E}
    Geschlecht[1] (0x15A - 0x15B)
       0 == männlich,  1 == weiblich, 2 divers, 3 unbekannt
- {0x14E - 0x150}
    Checksum[2] (0x15B - 0x15D)
        Checksum zur überprüfung vom Block

Ende vom Block [3]
    `FF FF FF`
    
# Kartenstatuswerte [8] (0x160 - 0x168)

- {0x00 - 0x01}
    Level[1] (0x160 - 0x0161)
        Das Level von der Karte
- {0x01 - 0x02}
    Angriff[1] (0x161 - 0x162)
        Wie stark die Karte ist im Angriff
- {0x02 - 0x03}
    Verteidigung[1] (0x162 - 0x163)
        Wie hoch der Verteidigungswert ist
- {0x03 - 0x04}
    Schnelligkeit[1] (0x163 - 0x164)
        Wie schnell die Karte ist (davon hängt ab wer Anfängt)
- {0x04 - 0x06}
    LP[2] (0x164 - 0x166)
        Wie viele Lebenspunkte die Karte hat
- {0x06 - 0x08}
    Checksum[2] (0x166 - 0x168)
        Checksum zur Überprüfung vom Block

Platzhalter [2]
    `FF FF`

# Kartenattacken (0x16A - 0x174) [6]

- {0x00 - 0x02}
    AttackenID 1 [2] (0x16A - 0x16C)
        Die AttackenID von der 1. Karten Attacke
- {0x02 - 0x04}
    AttackenID 2 [2] (0x16C - 0x16E)
        Die AttackenID von der 2. Attacke
- {0x02 - 0x04}
    AttackenID 3 [2] (0x16E - 0x170)
        Die AttackenID von der 2. Attacke
- {0x02 - 0x04}
    AttackenID 4 [2] (0x170 - 0x172)
        Die AttackenID von der 2. Attacke
- {0x04 - 0x06}
    Checksum[2] (0x172 - 0x174)
        Checksum zur Überprüfung

Platzhalter [3]
    `FF FF FF`

# Erlernbare Attacken (0x177 - 0x1D3)

96 Bytes lang


Eine erlernbare Attacke ist aufgebaut aus 3 bytes:

Bsp:
```
,______________________________,
|  Level [1] |  AttackenID [2] |
|------------|-----------------|
|    0x05    |      0x0004     |
|------------------------------|
```

Rest wird mit [0x00, 0x0000] gefüllt bis 96 bytes erreicht sind