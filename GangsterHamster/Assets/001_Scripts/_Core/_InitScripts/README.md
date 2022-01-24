# OpenRC

리눅스의 init 시스템을 참고해 만든 유니티 init 시스템

* * *

## 사용법

- OpenRC Init Script 를 생성
- Action 을 추가
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
OnGameStart   // OpenRC 가 생성되었을 시 호출됨
OnGameExit    // OpenRC 가 Destroy 될때 호출됨
OnSceneLoad   // Scene 이 로드되었을 시 호출됨    (미구현)
OnSceneUnLoad // Scene 이 언로드 되었을 시 호출됨 (미구현)
```

* * *

### InitScript

```cs
public RunLevel _RunLevel
// 실행될 레벨
```
```cs
public string _Name
// InitScript 의 이름
```
```cs
public UnityEvent<MonoBehaviour> Depend
// Start 가 호출되기 전 호출됨
```
```cs
public UnityEvent Start
// 지정된 RunLevel 에 호출됨
```
```cs
public UnityEvent Stop
// OpenRC 종료 시 호출 (OnDestroy)
```