from typing import Optional
import tiktoken
from transformers import AutoTokenizer


MODEL_TOKENIZER_MAP = {
    "gpt-3.5-turbo": "cl100k_base",
    "gpt-4": "cl100k_base",
    "llama-3": "meta-llama/Llama-3.1-8B",
    "llama-3.70": "meta-llama/Llama-3.1-70B",
    "llama-2": "NousResearch/Llama-2-7b-chat-hf",
    "mixtral-8x7b": "mistralai/Mixtral-8x7B-Instruct-v0.1",
}

def count_tokens(prompt: str, model: str) -> Optional[int]:
    model_key = model.lower()
    if model_key not in MODEL_TOKENIZER_MAP:
        print(f"[Fehler] Unbekanntes Modell: {model}")
        return None

    tokenizer_info = MODEL_TOKENIZER_MAP[model_key]
    try:
        if "gpt" in model_key:
            encoding = tiktoken.get_encoding(tokenizer_info)
            tokens = encoding.encode(prompt)
            return len(tokens)
        else:
            tokenizer = AutoTokenizer.from_pretrained(tokenizer_info)
            tokens = tokenizer.encode(prompt, add_special_tokens=False)
            #tokens = tokenizer.tokenize(prompt)
            return len(tokens)
    except Exception as e:
        print(f"[Fehler] Tokenisierung fehlgeschlagen für Modell '{model}': {e}")
        return None


if __name__ == "__main__":
    test_prompt = "hfbdjh hvdbjvkd jbvjkdbv djkbckdjnc vbdjhbvjkdkn. hdbcjdnckjndkjcb dhbchjdbcjk. chbxjcb cjkbdkcjn ncbcjhbdjkc hdbhdbncj dbchjbsxjkc h hvkhdu dbuzgd zcdgczud tf 67fgszu czs fctszg z dfuzsauzgszudhudgzuh suczjhbcztdfczshc. zgdzsuhuh suzgzf. jhasbghjsbc uhciush u.  zx uz gxuz g t tzc cuz ucz zu suzg z cc zus csuzshucsc cusg uzssgusc iusi 8fdtqghu zf w jhcbjkd"
    test_models = ["llama-3.70", "gpt-3.5-turbo", "gpt-4", "llama-3", "llama-2", "mixtral-8x7b"]

    for model in test_models:
        count = count_tokens(test_prompt, model)
        print(f"{model}: {count} Tokens")




