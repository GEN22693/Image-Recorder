# Image-Recorder

Klone das Repository mit folgendem Befehl:

```
git clone https://github.com/GEN22693/Image-Recorder
```

Mit der Datei **resetGit.cmd** kann das lokale Repo zurückgesetzt werden.

## Kurzbeschreibung

Dieses Projekt erstellt eine Vue.js Komponente, mit dessen Hilfe ein Bild von der Kamera des
Smartphones aufgenommen werden kann. Dabei sind folgende Aufgaben umzusetzen:
- Das Livebild wird angezeigt.
- Ein Button *Record* nimmt den aktuellen Frame auf.
- Nach der Aufnahme kann ein Marker (Rechteck) in das Bild gezeichnet werden, um den Schaden deutlicher
  erkennen zu können.
- Es gibt die Möglichkeit, Helligkeit und Kontrast anpassen zu können.
- Ein Button *Neue Aufnahme* verwirft das aktuelle Bild.
- Ein Button *Speichern* sendet den JPEG Stream über *emit()*, sodass das aufrufende Programm
  die Daten weiterverarbeiten kann.

## Teammitglieder

| Name                        | Email                    | Aufgabenbereich |
| --------------------------- | ------------------------ | --------------- |
| Lukas *Schneemayer*, 3CHIF  | sch22539@spengergasse.at | TBD             |
| Stefan *Trendafilov*, 3CHIF | tre22038@spengergasse.at | TBD             |
| Theodor *Gentschev*, 3CHIF  | gen22693@spengergasse.at | TBD             |

## Starten des Programmes

Gehe in das Verzeichnis *src* und führe die folgenden Befehle aus:

```
npm install
npm run dev
```
