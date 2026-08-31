# Copy Code Reference

Visual Studio 2022 확장이다. 코드 편집기에서 선택한 코드를 파일 경로와 줄 번호와 함께 클립보드에 복사한다. 절대 경로와 솔루션 상대 경로 중에서 고를 수 있다.

코드 리뷰, 이슈 작성, AI 코딩 에이전트에 코드 조각을 전달할 때 "어느 파일 몇 번째 줄인지"를 매번 손으로 적지 않아도 된다.

## 요구 사항

- Visual Studio 2022 (버전 범위 `[17.0,18.0)`, `amd64`)
- Windows
- 확장을 직접 빌드하려면 Visual Studio Installer 에서 `Visual Studio extension development` 워크로드 필요

## 설치 방법

- 빌드 산출물 `bin\Release\CopyCodeReference.vsix` 를 더블클릭해 VSIX Installer 로 설치한다.
- 설치 후 Visual Studio 를 재시작한다.

## 사용 방법

- 첫째, Visual Studio 편집기에서 코드를 선택한다.
- 둘째, 아래 둘 중 하나를 실행한다.
  - 편집기에서 우클릭 → `Copy Code Reference` 또는 `Copy Code Reference (Relative Path)`
  - `Edit` 메뉴 → 같은 두 항목
- 셋째, 원하는 곳에 붙여넣는다.

## 명령 두 개

| 명령 | 경로 | Keyboard 검색 이름 |
| --- | --- | --- |
| `Copy Code Reference` | 절대 경로 | `Edit.CopyCodeReference` |
| `Copy Code Reference (Relative Path)` | 솔루션 상대 경로 | `Edit.CopyCodeReferenceRelative` |

상대 경로의 기준은 솔루션 파일이 있는 디렉터리다. 아래 경우에는 절대 경로로 자동 대체한다.

- 솔루션이 열려 있지 않다.
- 파일이 솔루션 디렉터리 밖에 있다. 드라이브가 다른 경우도 포함한다.

`..\..\` 형태로 거슬러 올라가는 경로는 만들지 않는다. 그런 경로는 짧지도 않고 읽기도 어렵다.

기본 단축키는 지정하지 않는다. 필요하면 `Tools` → `Options` → `Environment` → `Keyboard` 에서 위 표의 이름을 검색해 직접 할당한다.

## 출력 예

한 줄만 선택한 경우. 경로와 줄 번호 뒤에 공백 한 칸을 두고 선택한 텍스트가 그대로 이어진다.

```text
D:\Project\SampleApp\ViewModels\MainViewModel.cs:42 var data = await repository.LoadAsync();
```

여러 줄을 선택한 경우. 위치만 복사하고 코드는 넣지 않는다.

```text
D:\Project\SampleApp\ViewModels\MainViewModel.cs:42-46
```

`Copy Code Reference (Relative Path)` 로 실행한 경우. 솔루션 디렉터리가 `D:\Project\SampleApp` 일 때다.

```text
ViewModels\MainViewModel.cs:42 var data = await repository.LoadAsync();
```

## 동작 규칙

- 선택한 코드 텍스트는 한 줄 선택일 때만 포함한다. 여러 줄을 선택하면 `경로:시작-끝` 한 줄만 복사한다.
- 한 줄 선택의 구분자는 공백 정확히 한 칸이다.
- 경로 형태는 실행한 명령이 정한다. 절대 경로 명령과 솔루션 상대 경로 명령이 따로 있다.
- 줄 번호는 1-based 다.
- 선택 영역의 끝이 다음 줄 첫 위치에 있어도 그 줄은 범위에 포함하지 않는다. 1~3 줄을 선택하면 `:1-3` 이지 `:1-4` 가 아니다.
- 선택한 텍스트에는 어떤 가공도 하지 않는다. 들여쓰기, 탭, CRLF, 줄 끝 공백을 그대로 유지한다.
- 성공해도 팝업이나 MessageBox 를 띄우지 않는다. 상태 표시줄에 짧은 메시지만 표시한다.

## 명령이 아무 일도 하지 않는 경우

아래 상황에서는 클립보드를 건드리지 않고 조용히 종료한다. 예외를 던지지 않는다.

- 선택 영역이 없다. 캐럿이 있는 줄을 자동으로 복사하지 않는다.
- 활성 문서나 텍스트 뷰를 얻을 수 없다. 디자이너, XAML 디자이너, 리소스 편집기, 도구 창 등이 활성인 경우다.
- 문서에 실제 파일 경로가 없다. 저장하지 않은 새 파일, 임시 문서가 여기 해당한다.
- 선택 영역이 가상 공백만 덮고 있어 실제 문자 범위가 비어 있다.
- 클립보드 접근이 실패했다. 다른 프로세스가 클립보드를 점유한 경우이며, 짧게 재시도한 뒤 포기한다.

## 빌드 방법

- Visual Studio 2022 로 `CopyCodeReference.sln` 을 연다.
- `Build` → `Build Solution` 을 실행한다. NuGet 패키지는 복원 시 자동으로 받아진다.
- Debug 와 Release 모두 빌드된다. VSIX 는 각 구성의 `bin\<Configuration>\CopyCodeReference.vsix` 에 생성된다.

명령줄로 빌드하려면 개발자 명령 프롬프트에서 아래를 실행한다.

```powershell
msbuild CopyCodeReference.sln /p:Configuration=Release /restore
```

## Experimental Instance 실행 방법

- 솔루션 탐색기에서 `CopyCodeReference` 프로젝트를 시작 프로젝트로 지정한다.
- `F5` 를 누른다. 별도의 Visual Studio Experimental Instance 가 뜬다.
- 그 인스턴스에서 코드 파일을 열고 `Edit` → `Copy Code Reference` 를 실행해 동작을 확인한다.

Experimental Instance 는 평소 쓰는 Visual Studio 설정과 분리된 별도 환경이라, 확장을 시험해도 일상 작업 환경이 오염되지 않는다.

## 테스트

`tests\CopyCodeReference.Tests` 프로젝트가 Visual Studio SDK 에 의존하지 않는 순수 로직을 검증한다.

- `RelativePathResolver` 의 솔루션 상대 경로 변환. 하위 폴더, 접두사 겹침 오탐, 다른 드라이브, UNC, 한글 경로, 대소문자 차이.
- `CodeReferenceBuilder` 의 출력 형식. 단일 줄 공백 구분자, 여러 줄 위치 전용, 빈 문자열, 들여쓰기 유지, 탭 유지, CRLF 유지, 한글 경로, Unicode 텍스트.
- `LineRangeCalculator` 의 줄 범위 계산. exclusive end 처리, 파일 마지막 줄, 끝 개행, 경계 클램프.

```powershell
dotnet test tests\CopyCodeReference.Tests\CopyCodeReference.Tests.csproj
```

## 현재 제약 사항

- Box / Column selection 은 공식 지원 범위가 아니다. 충돌하지는 않지만 stream selection 으로 환산한 범위를 복사한다.
- Multi-caret 은 첫 stream selection 만 대상으로 한다.
- 출력 형식은 고정이다. 설정 화면이 없다.
- ARM64 는 v0.1 공식 지원 범위가 아니다.

## 향후 기능

- Markdown 코드 펜스 출력 형식
- 선택이 없을 때 현재 줄 복사
- 사용자 출력 형식 설정
- Box selection 전용 처리
- Git 저장소 기준 상대 경로
- GitHub permalink 생성

## 개인정보

텔레메트리를 수집하지 않고 소스 코드를 외부로 전송하지 않는다. 네트워크 통신, 계정 로그인, 외부 API 호출이 없으며 Windows 클립보드 밖에는 아무것도 쓰지 않는다.

## 라이선스

MIT License. `LICENSE` 파일 참고.
