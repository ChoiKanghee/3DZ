# 3DZ

A small 3D shooter sandbox (Unity 2022.3).

## Requirements
- Unity **2022.3.XXf1** (LTS)
- Git LFS enabled (`git lfs install`)

## Open & Play
1. Clone repo, open `3DZ/` in Unity Hub.
2. Open `Assets/_Project/Scenes/Demo.unity`.
3. Play. Controls: WASD, Mouse1 fire, R reload, 1/2 switch.

## Build (local)
- File → Build Settings → Windows (x86_64) → Build.

## CI (GitHub Actions)
- See `.github/workflows/build.yml` – builds on push/PR (GameCI).

## Folder Structure
- `Assets/_Project/{Scripts, Prefabs, Scenes, Art, Audio, Settings}`
- `Assets/_ThirdParty` for external assets

## License
MIT (see LICENSE)
