import sys

from pyllamacpp.model import Model

word = ""
canWrite = False

def new_text_callback(text: str):
    global word
    global canWrite
    
    word += text

    if canWrite == True:
        print(text, end="", flush=True)
        
    if word.endswith("###BOT:"):
        canWrite = True

model = Model(ggml_model='' + sys.argv[1] + '', n_ctx=512)
    
while True:
    myInput = input('> ') 
    
    canWrite = False
    word = ""
    
    model.generate(
    "\n###USER: " + myInput + "\n###BOT: ",
    n_predict=512,
    verbose = False,
    new_text_callback=new_text_callback,
    temp=0.1,
    top_k=40,
    top_p=0.90,
    repeat_penalty=1.3,
    repeat_last_n = 64,
    n_threads=4,
    #antiprompt=["###USER:"],
    n_batch=5,
    seed=1680990864)
    
    print("\n", end="", flush=True)