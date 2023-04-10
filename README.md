# Jarvis
## GPT4ALL .net wrapper

Get the whisperer models from here: https://github.com/ggerganov/whisper.cpp

Get gpt4all-lora-quantized.bin and the .exe from here https://github.com/nomic-ai/gpt4all

<br />

In the TheBot class:

1. Change GPT4All_Exe_Location and GPT4ALL_Model_Location to the desired location.

2. Change whisperFactory location.

<br />
Usage:

Press R and say something on the mic. The input will be processed by whisperer and then sent to GPT4ALL.

<br />

### Testing the v01 build:

Place ggml-model-whisper-base.bin and gpt4all-lora-quantized.bin inside the folder. 

<br />

## Web Version JarvisOnTheWeb
MVC net 6 with signalR

![image](https://user-images.githubusercontent.com/11161818/230750328-7465d6fc-a651-4c80-bee6-0a34e51799ac.png)

<br />

## Python Pyllamacpp script

Added a script to run the pyllama library for custom ggml format.

Instruction to install pyllamacpp: https://github.com/nomic-ai/pyllamacpp

Example model: https://huggingface.co/Pi3141/gpt4-x-alpaca-native-13B-ggml

<br/>

To run python script instead of the GPT4ALL executable, change the following properties:

[Api.cs](https://github.com/graposo1/Jarvis/blob/master/JarvisInTheWeb/Api.cs)

```csharp
private bool IsCustomPython = true;
private string Python_Model_Location = AppDomain.CurrentDomain.BaseDirectory + "ggml-model-q4_1.bin";
```



