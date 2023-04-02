# Jarvis
GPT4ALL .net wrapper

Get the whisperer models from here: https://github.com/ggerganov/whisper.cpp and place them inside the "models" folder.

Get gpt4all-lora-quantized.bin and the .exe from here https://github.com/nomic-ai/gpt4all.


In the TheBot class:

1. Change GPT4All_Exe_Location and GPT4ALL_Model_Location to the desired location.

2. Change whisperFactory location.

Usage:

Press R and say something on the mic. The input will be processed by whisperer and then sent to GPT4ALL.
