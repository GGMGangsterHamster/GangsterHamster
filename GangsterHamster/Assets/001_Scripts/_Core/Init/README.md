# OpenRC

리눅스의 init 시스템을 참고해 만든 유니티 init 시스템<br/>
<b>실행 순서 보장되지 않음</b>

* * *

## 사용법

- OpenRC Init Script 를 생성
- ExampleOpenRCInitScript.cs 를 기반으로 스크립트를 작성
- OpenRC Init Script 에 작성한 스크립트를 추가
- OpenRC::initScripts 에 추가

* * *

### 함수들 (OpenRC)

```cs
int Add(InitScript initScript)
// initScript 를 initScript._RunLevel 에 실행되게 함
```

```cs
int Del(InitScript initScript)
// initScript 를 initScript._RunLevel 실행에서 제외함
```

### Enum (RunLevel)

```cs
OnGameStart           // OpenRC 가 생성되었을 시 호출됨
OnGameExit            // OpenRC 가 Destroy 될때 호출됨
OnSceneLoad           // Scene 이 로드되었을 시 호출됨
OnSceneUnLoad         // Scene 이 언로드 되었을 시 호출됨
```

* * *

### InitScript

```cs
RunLevel _RunLevel
// 실행될 레벨
```
```cs
string _Name
// InitScript 의 이름
```
```cs
UnityEvent<MonoBehaviour> Depend
// Start 가 호출되기 전 호출됨
```
```cs
UnityEvent<object> Start
// object: 필요한 경우 전달됨
// 지정된 RunLevel 에 호출됨
```
```cs
UnityEvent Stop
// OpenRC 종료 시 호출 (OnDestroy)
```