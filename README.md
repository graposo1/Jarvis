# Jarvis
## GPT4ALL .net wrapper

Get the whisperer models from here: https://github.com/ggerganov/whisper.cpp

Get gpt4all-lora-quantized.bin and the .exe from here https://github.com/nomic-ai/gpt4all


In the TheBot class:

1. Change GPT4All_Exe_Location and GPT4ALL_Model_Location to the desired location.

2. Change whisperFactory location.

Usage:

Press R and say something on the mic. The input will be processed by whisperer and then sent to GPT4ALL.

## Testing the v01 build:

Place ggml-model-whisper-base.bin and gpt4all-lora-quantized.bin inside the folder. 
