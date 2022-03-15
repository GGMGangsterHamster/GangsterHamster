# Callback
### 스크립트가 지원한다면 실행되는 Callback
<br/>

* * *
<br/>

### 사용
```cs
1. ICallbackable 을 Callback 하고 싶은 클레스에 구체화
2. ICallbackable 을 호출할 수 있게 구현된 클래스 자식 오브젝트로 넣어 둠
3. 잘 작동하는지 관찰
```

* * *

### 유틸 구조
```cs
public static class ExecuteCallback {
    // 자식 Callback 호출 기능 (하나만 호출함)
    public static void Call(Transform transform, object param = null);
}
```