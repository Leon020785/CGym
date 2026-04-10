# Copilot Instructions

## Project Guidelines
- User prefers short, concise code documentation written in Danish. Avoid long XML doc comments.
- Provide step-by-step code guidance. Include a comment line (//) above each line explaining what each line means, but only when explicitly requested. For missing code, provide clear explanations of why and where to place it.
- When the user requests help, do not change code directly; create a step-by-step plan and guide the user through one part at a time. Lever kun en plan/prompt i chatten, så brugeren selv udfører ændringerne trin for trin.
- Do not change code directly. Instead, explain step by step what the user should change.
- Treat `User` and `Member` as the same person/profile. The user should be able to book activities immediately after login without a separate `Member` registration.