import subprocess
import sys
import os

def install_package(package):
    try:
        __import__(package)
    except ImportError:
        print(f"正在安装依赖库：{package}")
        subprocess.check_call([sys.executable, "-m", "pip", "install", package])

install_package("pandas")
install_package("openpyxl")

import pandas as pd

INPUT_DIR = "Tables"
OUTPUT_DIR = "Data"

os.makedirs(OUTPUT_DIR, exist_ok=True)

for filename in os.listdir(INPUT_DIR):
    if filename.endswith(".xlsx") and not filename.startswith("~$"):
        excel_path = os.path.join(INPUT_DIR, filename)
        json_name = os.path.splitext(filename)[0] + ".txt"
        json_path = os.path.join(OUTPUT_DIR, json_name)
        
        try:
            df = pd.read_excel(excel_path)

            # 将特定列转换为int类型
            if 'nextbranchid' in df.columns:
                df['nextbranchid'] = df['nextbranchid'].fillna(0).astype(int)
            if 'option1next' in df.columns:
                df['option1next'] = df['option1next'].fillna(0).astype(int)
            if 'option2next' in df.columns:
                df['option2next'] = df['option2next'].fillna(0).astype(int)
            if 'option3next' in df.columns:
                df['option3next'] = df['option3next'].fillna(0).astype(int)
            if 'option4next' in df.columns:
                df['option4next'] = df['option4next'].fillna(0).astype(int)

            df.to_json(json_path, orient="records", force_ascii=False, indent=4)
            print(f"转换成功：{json_path}")
        except Exception as e:
            print(f"失败：{str(e)}")

