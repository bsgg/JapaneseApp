=================
EasyTTS (TextToSpeech)
=================
## [Overview]
It is a plug-in that allows you to easily use the native text-to-speech feature of iOS and Android.

## [System Requirement]
iOS7 or more
Android 1.6 or more, In Android TTS Engine must installed.


##[How to use and Detailed Description]
Use EasyTTSUtil.Speech (a string that converts into speech)in which perform a text-to-speech.

Initializing the text to speech: EasyTTSUtil.Initialize();
And
To end the process: EasyTTSUtil.Stop ();
It is needed so that text to speech will perform.


"This is only for Android."
In Android, user can switch other TTS engine to change option of Voice Setting and Language for the inputed text.
Theres a case that in changing Voice Setting and Language will depend on the installed TTS engine.

EasyTTSUtil.Initialize (language, engine); 

Use the ISO 639-1 language code for English en, Japanese is ja. 
engine,is the package name of the installed TTS engine.

EasyTTSUtil.GetEnginePkgArray()
With this method, you can get the list of TTS names that are installed in device.

In addition, a method that opens a screen for voice setting of the device:
EasyTTSUtil.OpenTTSSetting()
Since it can also be used, please use if its needed.
-------------------------------------------------------------------------
## [Demo]
DemoEasyTTS.unity
For sample scene that demonstrate the inputed text to speech.

DemoEasyTTS_SelectEngine
Sample scene for Android with select option for the installed TTS .

## [How to install]
Import the package, EasyTTS.unitypackage

①Need to put this files on your project:
Plugins / Android / EasyTTSUtil.jar 
Plugins / iOS / EasyTTSUtil.mm
Plugins / iOS / EasyTTSUtil.h 
It is used to bundle all components and resources into a single file to distribute application software or libraries.

②Plugins(folder):Please make sure that is included in your project.


##[Publisher]

FreCre Inc.

If theres a bug and questions please contact: 
customersupport@frecre.com
We will correspond within the range that can be done.

## [Reference]
Android-reference
http://developer.android.com/reference/android/speech/tts/TextToSpeech...

iOS
AVSpeechSynthesizer