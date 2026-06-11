# 🖱️ Visual Studio Git Commit - Step-by-Step with Clicks

## 📌 Pre-Commit Setup

### **Step 0: Ensure Solution Builds**
```
Keyboard: Ctrl + Shift + B (Build Solution)
Look for: "Build succeeded" in Output window
```

---

## 🎯 Main Steps to Commit

### **STEP 1: Open Git Changes Window**

**Location 1: Keyboard Shortcut**
```
Press: Ctrl + 0, Ctrl + G
(Hold Ctrl, press 0, release all, press Ctrl + G)
```

**Location 2: Menu**
```
Menu Bar → View
    ↓
Look for: "Git Changes"
    ↓
Click: "Git Changes"
```

**Location 3: Team Explorer (Alternative)**
```
Menu Bar → View
    ↓
Look for: "Team Explorer"
    ↓
Click: "Team Explorer"
    ↓
In Team Explorer panel, click: Home
    ↓
Then click: Changes
```

✅ **You should see:**
- Left panel with file list
- Top section with commit message box
- "Staged Changes" area
- "Untracked Files" area

---

### **STEP 2: Stage All Files**

**Option A: Stage All Button (EASIEST)**
```
Look for the button that says: "Stage All" or "⊕ Stage All"
Click: That button
```

**Option B: Right-Click Files**
```
In the file list:
    ↓
Right-click each file
    ↓
Select: "Stage" 
    ↓
Repeat for all files
```

**Option C: Drag & Drop**
```
From "Untracked Files" section
    ↓
Drag files down to
    ↓
"Staged Changes" section
```

✅ **After Staging:**
- All files should show under "Staged Changes"
- "Untracked Files" should be empty

---

### **STEP 3: Enter Commit Message**

**Click in the Text Box**
```
Look for: Text box with placeholder "Add a message..."
    ↓
Click: Inside the text box
    ↓
You should see: Cursor blinking
```

**Type the Title Line**
```
Type: feat: Add 3 innovation features - Notifications, Analytics, Admin Reports
```

**Press Enter Twice**
```
Press: Enter (2 times)
Result: Creates space for description
```

**Paste the Description**
```
Copy from READY_TO_COMMIT.md (the full description)
    ↓
Right-click in message box
    ↓
Select: "Paste"
    ↓
Or: Ctrl + V
```

✅ **Your commit message should look like:**
```
feat: Add 3 innovation features - Notifications, Analytics, Admin Reports

Add comprehensive innovation features and fix ApplicationUser properties

FEATURES ADDED:

1. Real-Time Notifications System
...
```

---

### **STEP 4: Create the Commit**

**Look for Commit Button**
```
In the Git Changes panel:
    ↓
Look for button that says:
  - "Commit Staged" or
  - "Commit All" or
  - "✓ Commit"
    ↓
Click: That button
```

**You should see:**
```
Message: "Commit created successfully"
Or: "Commit pushed successfully"
```

✅ **Commit is complete!**

---

### **STEP 5: Push to GitHub (Optional but Recommended)**

**Find Push Button**
```
In Git Changes panel:
    ↓
Look for: "⬆ Push" button
    ↓
Or: Use keyboard shortcut Ctrl + Shift + K
    ↓
Click: Push button
```

**GitHub Notification**
```
You might see: "Sign in to GitHub"
    ↓
If so: Sign in with your credentials
    ↓
Then push proceeds
```

✅ **After Push:**
- Your changes are on GitHub
- You can see them at github.com/jaywinwaterboer03-dot/SomaShare_WEBB262_SS2

---

## 🔍 Verification Steps

### **VERIFY 1: Check Commit in Team Explorer**

```
Team Explorer → Home
    ↓
Click: "Changes"
    ↓
Result: Should show "No changes" or "0 files"
    ↓
This means everything is committed ✅
```

### **VERIFY 2: Check Commit History**

```
Team Explorer → Home
    ↓
Click: "History"
    ↓
You should see:
    - Your new commit at the TOP
    - Title: "Add 3 innovation features..."
    - Shows current time
    - Shows your GitHub username
    ↓
Click on it: See all files changed
```

### **VERIFY 3: Check on GitHub Website**

```
Open: https://github.com/jaywinwaterboer03-dot/SomaShare_WEBB262_SS2
    ↓
Look for: Green "Code" button
    ↓
Near there: "X commits" link
    ↓
Click: That link
    ↓
You should see:
    - Your new commit at TOP
    - All 19 new files listed
    - All 5 modified files listed
    - Commit message visible
```

---

## 📸 What Each Window Looks Like

### **Git Changes Window - Before Staging**
```
┌─ Git Changes ──────────────────────────────┐
│                                             │
│ Untracked Files (19):                       │
│  □ Notification.cs                          │
│  □ NotificationService.cs                   │
│  □ AnalyticsService.cs                      │
│  □ ...more files...                         │
│                                             │
│ Changes (5):                                │
│  □ Program.cs                               │
│  □ ApplicationDbContext.cs                  │
│  □ ...more files...                         │
│                                             │
│ [⊕ Stage All]                              │
│                                             │
└─────────────────────────────────────────────┘
```

### **Git Changes Window - After Staging**
```
┌─ Git Changes ──────────────────────────────┐
│                                             │
│ Message: feat: Add 3 innovation features... │
│          [paste full message here]          │
│                                             │
│ [✓ Commit Staged] [⬆ Push]                 │
│                                             │
│ Staged Changes (24):                        │
│  ✓ Notification.cs                          │
│  ✓ NotificationService.cs                   │
│  ✓ AnalyticsService.cs                      │
│  ✓ ...all 19 new files...                  │
│  ✓ Program.cs                               │
│  ✓ ApplicationDbContext.cs                  │
│  ✓ ...all 5 modified files...              │
│                                             │
│ Untracked Files: (empty)                    │
│ Changes: (empty)                            │
│                                             │
└─────────────────────────────────────────────┘
```

### **Team Explorer History - After Commit**
```
┌─ Team Explorer ────────────────────────────┐
│ Home                                        │
│  ├─ Changes (0 files)                      │
│  ├─ Branches                               │
│  ├─ History                                │
│  └─ Settings                               │
│                                             │
│ History:                                    │
│  1. feat: Add 3 innovation features...     │
│     jaywinwaterboer03-dot                   │
│     Just now (or 1 minute ago)             │
│     ├─ Added: Data/Notification.cs         │
│     ├─ Added: Data/Services/...            │
│     ├─ Modified: Program.cs                │
│     └─ ... 19 more files ...               │
│                                             │
│  2. [Previous commits...]                  │
│                                             │
└─────────────────────────────────────────────┘
```

---

## ⏱️ Time Estimate

```
Step 1: Open Git Changes        → 10 seconds
Step 2: Stage All Files         → 5 seconds
Step 3: Copy/Paste Message      → 30 seconds
Step 4: Click Commit            → 2 seconds
Step 5: Wait for commit         → 3 seconds
Step 6: Push to GitHub          → 10 seconds

TOTAL TIME: ~1 minute
```

---

## 🆘 Troubleshooting - What to Do If...

### **"Git Changes is greyed out"**
```
Solution:
  1. Restart Visual Studio
  2. Or ensure project is open
  3. Or check if you're in correct directory
```

### **"Can't find Stage All button"**
```
Solution:
  1. Try keyboard: Right-click file → Stage
  2. Or drag files to staging area
  3. Or use command palette: Ctrl + Shift + P
```

### **"Commit button is disabled"**
```
Solution:
  1. Ensure files are staged
  2. Ensure commit message has text
  3. Check if git is properly configured
```

### **"See 'No changes' message"**
```
Solution:
  1. Refresh: Press F5 in Team Explorer
  2. Or restart Visual Studio
  3. Or check if files are actually modified
```

### **"GitHub authentication fails"**
```
Solution:
  1. Go to Team Explorer → Settings
  2. Under Git Global Settings:
     - User Name: your GitHub username
     - Email: your GitHub email
  3. Try again
```

---

## ✅ Final Pre-Commit Checklist

Before clicking "Commit", make sure:

```
□ Solution builds (Ctrl + Shift + B)
□ Build shows: "Build succeeded"
□ Git Changes window is open
□ All files are staged
□ "Untracked Files" is empty
□ Commit message is filled in
□ Commit message starts with "feat:"
□ Commit message has description
□ You're on "master" branch
□ You're signed into GitHub
```

---

## 🎯 Quick Reference: Button Locations

### **Visual Studio 2026 Locations**

**To open Git Changes:**
```
Keyboard: Ctrl + 0, Ctrl + G
Menu: View → Git Changes
```

**To stage all:**
```
Button: ⊕ Stage All (in Git Changes window)
Or: Right-click files → Stage
```

**To commit:**
```
Button: ✓ Commit Staged (in Git Changes window)
Keyboard: (No direct shortcut, use button)
```

**To push:**
```
Button: ⬆ Push (in Git Changes window)
Keyboard: Ctrl + Shift + K
Menu: Git → Push to Remote Branch
```

**To view history:**
```
Team Explorer: Click History tab
Menu: View → Team Explorer → History
```

---

## 📋 The Exact Steps (Ultra-Quick Version)

1. **Ctrl + 0, Ctrl + G** → Opens Git Changes
2. **Click ⊕ Stage All** → Stages all files
3. **Paste commit message** → In message box
4. **Click ✓ Commit Staged** → Creates commit
5. **Click ⬆ Push** → Sends to GitHub
6. **Verify on github.com** → Commit appears

**Done!** ✅

---

## 🎓 After You Commit

**What to do with your commit:**

1. ✅ **Copy the commit URL:**
   ```
   https://github.com/jaywinwaterboer03-dot/SomaShare_WEBB262_SS2/commit/[hash]
   ```

2. ✅ **Include in SS3 documentation:**
   - Add to README.md
   - Add to SS3_INNOVATION_SUMMARY.md
   - Screenshot for submission

3. ✅ **Screenshot for SS3:**
   - Take screenshot of Team Explorer History
   - Take screenshot of GitHub commit page
   - Add to Word document

4. ✅ **Share with lecturer:**
   - Include commit link in submission
   - Show meaningful commit history
   - Demonstrates version control usage

---

**You're ready to commit!** 🚀

**Next Step:** Follow Step 1 above and start the commit process!
