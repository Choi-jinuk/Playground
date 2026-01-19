# Playground
개인 프로젝트 연습

## 프로젝트 구조
```
.
├─ Packages/
│  └─ com.playground.framework/
│     ├─ package.json
│     ├─ README.md
│     ├─ CHANGELOG.md
│     ├─ LICENSE.md
│     ├─ Runtime/
│     │  └─ Scripts/
│     │     ├─ Data/
│     │     ├─ Database/
│     │     ├─ Global/
│     │     ├─ Input/
│     │     ├─ Login/
│     │     ├─ Network/
│     │     ├─ Platform/
│     │     ├─ Runtime/
│     │     ├─ Scene/
│     │     ├─ Sound/
│     │     ├─ UI/
│     │     ├─ Util/
│     │     └─ Web/
│     ├─ Editor/
│     │  └─ Scripts/
│     ├─ Tests/
│     │  ├─ Runtime/
│     │  └─ Editor/
│     ├─ Samples~/
│     │  └─ Example/
│     └─ Documentation~/
└─ README.md
```

### 구성 요소 요약
- `Packages/com.playground.framework/Runtime`: 빌드에 포함되는 런타임 코드.
- `Packages/com.playground.framework/Editor`: Unity 에디터 전용 코드.
- `Packages/com.playground.framework/Tests`: 런타임/에디터 테스트 분리.
- `Samples~/`: 선택적으로 임포트 가능한 샘플.
- `Documentation~/`: 패키지 문서.

## 사용/활용한 플러그인 목록
- Cysharp/UniTask
- SimpleJSON
