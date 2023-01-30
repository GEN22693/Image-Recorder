# Follow up Project im Sommersemester: PocketGPT

## Problemstellung

Oft werden Tests ausgedruckt ausgeteilt und die Schüler müssen alle Notebooks zumachen. Daraus
ergibt sich das Problem, dass Chat GPT nur über das versteckte Smartphone verfügbar ist. Doch
die Angabe kann nur schwer eingegeben werden. OCR soll das Problem lösen.

## Features

Schüler starten die Webapp. Dort wird die Kamera angesprochen und man kann die Testangabe
fotografieren. Danach startet der OCR Prozess mit Tesseract.js. Der Text wird dann aufgeteilt
und jede Frage wird über die OpenAI API an die AI gesendet.

Die Antworten werden dann leicht lesbar angezeigt. Die einzelnen Fragen lassen sich durch swipes
aufrufen, sodass man unauffällig nachsehen kann.

Eine Erweiterung kann die Antworten der AI über einen Websocket an andere Schüler der Klasse
automatisch schicken. Das Abfotografieren ist nicht an jedem Sitzplatz gleich einfach, so kann
das Risiko verteilt werden.

## Technische Implementierung

### Komponente ImageRecognizer

- Erstelle eine View.js Komponente *ImageRecognizer*. Sie zeigt das Livebild an und bietet einen
  Button "Capture".
- Suche eine JS Library, die das Bild für die OCR Erkennung noch aufbereiten kann (ausrichten,
  Helligkeits- Kontrastkorrektur)
- Übergib die Bilddaten dem OcrService. Es liefert dann ein Array mit den Testfragen.
- Über *$emit* werden die Fragen dann an den Aufrufer als Daten des Events *onRecognize* übergeben.


### Serviceklasse OcrService.js

Lege im Ordner Services eine Datei *OcrService* an. Sie wird dann über import in die Komponente
*ImageRecognizer* eingebunden. Dieses Service bindet über einen Import
[Tesseract.js ](https://tesseract.projectnaptha.com/) ein.

```js
class OcrService {
  constructor() {

  }

  recognizeText(imageStream) {
    // TODO: Text recognition with tesseract and split questions.

    // Fake it till you make it
    return ["This is Question 1", "This is Question 2"]
  }
}

const ocrService = new OcrService();
export default ocrService;
```

### View

Die View bindet die Komponente *ImageRecognizer* ein und abonniert das Event.

```html
<image-recognizer v-bind:onRecognize="processQuestions"></image-recognizer>
```

Im Eventhandler wird dann die Anfrage an die OpenAI API gesendet. Um den Key zu bekommen,
wird im Backend eine Route */api/key* implementiert, die den Key zurückgibt.

Für die Entwicklung muss eine Fake-API z. B. unter */api/openai* bereitgestellt werden.
Sie liefert das selbe Datenformat wie die echte API, jedoch mit Mustertext. Dadurch wird
vermieden, dass bei der Entwicklung die Tokens aufgebraucht werden.

## Recherche

- **OpenAI API Key**: https://beta.openai.com/account/api-keys
- **OpenAI Text completition**: https://beta.openai.com/docs/api-reference/completions
- **OCR mit Javascript und Tesseract**: https://www.youtube.com/watch?v=IE4lnxXoupg

