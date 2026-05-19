---
name: decision-survey
description: This skill should be used when the user faces multiple technical approaches or architectural options and needs an interactive way to evaluate trade-offs and make a decision.
---

# Decision Survey Skill

## Purpose

Convert a multi-option technical decision into an interactive HTML survey page. The survey lets the user compare approaches side-by-side with pros/cons, code examples, and priority tags, then submit their choice. After submission, the AI receives the result and proceeds with implementation.

## When to Use

- The user asks about trade-offs between 2+ approaches and can't decide
- The user mentions "choice", "decide", "option", "方案", "选择哪个"
- The user asks to create an HTML page for decision making
- A technical discussion reaches a fork where multiple valid paths exist

## Workflow

### Step 1: Identify the Decision Scope

Extract from the conversation:
- What is the problem being solved?
- What are the candidate approaches?
- What are the criteria for evaluating them (complexity, performance, maintainability, etc.)?

If there are fewer than 2 options, this skill does not apply.

### Step 2: Build the Survey HTML

Create a single self-contained HTML file. The survey must include:

**Layout:**
- Left panel: Background explanation + rules/constraints in markdown-rendered style
- Right panel: The questionnaire form

**Questionnaire structure:**
1. Title and subtitle describing what's being decided
2. Each option as a card with:
   - Radio button for selection
   - Option name (bold title)
   - Badges/Tags (e.g., `推荐`, `P0`, `简单`, `灵活`)
   - Description (1-2 sentences)
   - Code snippet (3-8 lines, syntax highlighted with inline colors)
   - Pros and Cons tables
3. A notes/remarks textarea for user input
4. A submit button

**Design guidelines:**
- Clean, modern, responsive (flexbox layout)
- Use CSS variables for theming
- Badge colors: green for `推荐`, blue for `P0`/`推荐`, orange for `P1`, gray for `P2`
- Selected option has a blue border highlight
- All styling inline — no external dependencies
- Font: system-ui stack
- Code blocks use dark background (#1e1e1e) with color-coded syntax:
  - Keywords: #569cd6 (blue)
  - Strings: #ce9178 (orange)
  - Comments: #6a9955 (green)
  - Methods/functions: #dcdcaa (yellow)

**Behavior:**
- Clicking a card selects its radio button
- Submit button disabled until a choice is made
- On submit, show a confirmation message with the selected option and a prompt: "请将选择结果告诉我"
- Use localStorage to persist the choice (in case page is refreshed)

### Step 3: Collect the Result

After the user tells you their selection, proceed to implement the chosen approach. Update any relevant code in the workspace as needed.

If the user does not tell you after the HTML is created, wait — do not assume a choice.

### Step 4: Cleanup

After the decision is made and implementation is done, delete the temporary HTML file.

## Example

Given two approaches for handling Undo in Odin PropertyTree with pure C# objects:

**Approach A**: Suppress warning (`TreeIsSetupForIMGUIDrawing_TEMP_INTERNAL = true`)
**Approach B**: Manual dirty marking with `OnPropertyValueChanged` callback

The survey would present both with code examples, pros/cons, and let the user pick.

## Constraints

- The HTML must be a single file — no external CSS/JS/fonts
- Save the file to the workspace where the user can easily find it (e.g., alongside related code)
- Always include a notes/remarks field — the user may want to explain their reasoning
- After submission, the result should be clear enough for the AI to proceed without guessing
