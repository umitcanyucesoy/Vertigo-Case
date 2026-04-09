# Project Instructions

## Architecture Principles

- Build the project with a clear separation of responsibilities.
- Favor maintainable, reusable, and scalable systems over quick one-off solutions.
- Avoid over-engineering, but design systems so they can evolve safely.

## Services

- Use **services** only for shared systems and cross-cutting concerns.
- Services must **not** be MonoBehaviours / behaviours unless there is a very strong technical reason.
- Services are created and initialized through **GameBootstrapper**.
- GameBootstrapper should initialize services via their **interfaces**, using **ServiceLocator**.
- Not everything should become a service.
- Typical service examples:
    - Grid
    - Input
    - Pool
    - Audio
    - VFX
- Prefer lightweight, focused services with a single responsibility.

## Bootstrapper

- `GameBootstrapper` is responsible for wiring the project together.
- It should initialize services and core systems in a predictable order.
- Systems should expose initialization through `Init()` or a similarly explicit setup method.
- Avoid hiding important startup logic in random lifecycle methods.

## Controllers

- Core gameplay and game flow must be managed by **Controllers**.
- Controllers own the main mechanics and orchestration logic.
- If one controller needs to communicate with another, use **interface-based communication**.
- Apply **interface segregation**: expose only what another system actually needs.
- Prefer interfaces like `IGridController`, `ILevelController`, `ICombatController`, etc. instead of overly broad contracts.
- Controllers may directly manage mechanic-specific operations such as spawning, type assignment, renderer updates, and presentation-side gameplay handling when appropriate.

## Event Bus

- Use an **EventBus** for event-driven communication where multiple systems need to react to the same occurrence.
- Do **not** make everything event-driven.
- Only events that are truly broadcast-worthy and observed by multiple systems should go through EventBus.
- Avoid excessive event usage, because it creates **hidden flow** and makes debugging harder.
- Favor direct calls when the relationship is explicit and local.
- Use events for examples like:
    - game state changes
    - level lifecycle notifications
    - multi-system reactions
    - loosely coupled feature communication

## Data / Logic Separation

- Separate **data** from **logic** using **ScriptableObjects**.
- Use ScriptableObjects actively for configurable data, definitions, balancing, and reusable content.
- Keep runtime logic out of data assets.
- Prefer data-driven systems whenever it improves iteration speed and clarity.

## Patterns

- Favor the **Strategy Pattern** when behavior variation is needed.
- Abstract base classes can be used when justified, but do not introduce inheritance hierarchies unnecessarily.
- Reusability is important: build systems that can be extended without rewriting core logic.

## Async / Flow Management

- Avoid unnecessary per-frame polling and meaningless `Update()` loops.
- Do not rely on `Update()` for systems that can be modeled more clearly with async flows.
- Prefer **UniTask** with `async/await` for manageable gameplay flow, sequencing, waiting, and orchestration.
- Use frame-based callbacks only when they are truly required by the mechanic.

## Animation

- Use **DOTween** for animations and tween-driven flows.
- Prefer clean, explicit tween sequences over ad-hoc animation logic.

## Editor Tooling

- If custom editor-side data authoring is needed, write it with **pure Unity editor scripting**.
- Editor tooling can work together with ScriptableObjects for better authoring workflows.
- Keep editor-only code clearly separated from runtime code.

## Coding Expectations

- Write clean, readable, and reusable code.
- Keep responsibilities narrow and explicit.
- Prefer composition over unnecessary inheritance.
- Avoid god objects and overly broad managers.
- Avoid turning every system into a generic framework too early.
- Make dependencies visible and intentional.
- Optimize for maintainability, debuggability, and production clarity.

## What to Avoid

- Do not convert every system into an event.
- Do not convert every shared object into a service.
- Do not hide architecture decisions inside unrelated MonoBehaviours.
- Do not create large interfaces that expose unrelated responsibilities.
- Do not couple data assets with runtime logic.
- Do not introduce unnecessary `Update()`-driven systems when async orchestration is enough.