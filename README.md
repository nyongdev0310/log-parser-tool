# 📜 Log Parser & Analyzer (Console + WinForms)

A small but powerful utility that parses log files using regular expressions  
and exports structured results into a CSV file.

Perfect for demonstrating **Regex, text parsing, file IO, and tooling skills**.

---

## ✨ Features

- Parse `[INFO]`, `[WARN]`, `[ERROR]` lines  
- Extract log level + message  
- Skip irrelevant/unknown lines  
- WinForms UI with:
  - File chooser  
  - CSV save dialog  
  - Real‑time logs  
- Includes sample log data

---

## 🎥 Demo Video (YouTube)

[![Log Parser Demo](https://img.youtube.com/vi/U9jI7v6t96o/0.jpg)](https://youtu.be/U9jI7v6t96o)

You can watch a full demonstration of how the Log Parser works,  
including selecting the log file, parsing INFO/WARN/ERROR entries,  
and exporting them into a CSV file.

---

## 🧰 Tech Stack

- **C# / .NET**
- **Regex**
- **File IO**
- **WinForms**

---

## 📂 Project Structure

/log-parser-tool
/ConsoleVersion
/WinFormsVersion
/sample
log.txt
result.csv
/screenshots
01-main.png
02-selected-file.png
03-finished.png

---

## ▶ How to Use (WinForms)

1. Select `log.txt`  
2. Select CSV output file  
3. Click **Parse**  
4. Review results  

---

## ▶ Example Log Format

**Input:**

```text
[INFO] Application started
[WARN] Configuration file not found
[ERROR] Failed to connect to database

Some random line
[DEBUG] Debug message (ignored)

Output CSV:
Level,Message
INFO,Application started
WARN,Configuration file not found
ERROR,Failed to connect to database
```

---

## 📸 Screenshots
(Add your screenshots here)

---

## ⚠ Limitations

- Only INFO/WARN/ERROR supported
- DEBUG and other formats are ignored
- No timestamp extraction

---

## 🚀 Enhancements (Optional)

- Timestamp parsing
- Support DEBUG/TRACE
- Export JSON / Excel
- Log level filters in UI

---

## 📜 License
MIT License
