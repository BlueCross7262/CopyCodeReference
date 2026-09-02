# Marketplace 게시 자료

Visual Studio Marketplace 등록 화면에 그대로 붙여 넣을 값과, 사람이 직접 만들어야 하는 자료의 촬영 지침을 모아 둔다.

## 등록 화면 입력값

| 항목 | 값 |
| --- | --- |
| Internal Name | `CopyCodeReference` |
| Display Name | `Copy Code Reference` |
| Version | `0.1.3` |
| VSIX ID | `CopyCodeReference.8ac855e5-611b-4f49-b75f-8519d132f8b6` |
| Publisher | `cy.ryu` |
| Type | Tools |
| Pricing | Free |
| Visual Studio | 2022 |
| Editions | Community, Professional, Enterprise |
| Architecture | amd64 |
| Source Repository | https://github.com/BlueCross7262/CopyCodeReference |
| License | MIT |
| Logo | `assets/icon.png` |
| Overview | `docs/marketplace/overview.md` 내용 |

VSIX ID 와 Publisher 는 최초 공개 이후 변경하지 않는다. 자동 업데이트가 이 두 값으로 확장을 식별한다.

## Short Description

```text
Copy selected Visual Studio code with its file path and line numbers.
```

## Tags

```text
code, copy, reference, clipboard, selection, line number, file path, developer tools, visual studio
```

## 업로드할 VSIX

```text
bin\Release\CopyCodeReference.vsix
```

Debug 산출물을 올리지 않는다. GitHub Release `v0.1.0` 에 첨부된 파일과 같은 파일이어야 한다.

## 스크린샷 촬영 지침

Marketplace 상세 페이지에 최소 2장이 필요하다. Visual Studio 가 설치된 환경에서 직접 촬영해 아래 경로에 저장한다.

- `assets/marketplace/selection.png`
  - 에디터에서 코드 한 줄을 선택한 상태.
  - 왼쪽 줄 번호 여백이 보이게 잡는다. 줄 번호가 결과 문자열과 대응된다는 것이 한눈에 보여야 한다.
  - 가능하면 `Edit` 메뉴를 펼쳐 `Copy Code Reference` 항목이 같이 보이게 한다.
- `assets/marketplace/result.png`
  - 붙여넣은 결과. `경로:줄번호 공백 코드` 형태가 그대로 읽혀야 한다.
  - 여러 줄 선택 결과(`경로:시작-끝`)를 같은 이미지에 나란히 넣으면 두 모드 차이가 전달된다.

촬영 시 실제 사내 경로나 비공개 코드가 노출되지 않도록 샘플 프로젝트를 쓴다.

## 게시 전 사람이 직접 채워야 하는 항목

아래 두 가지는 Visual Studio 가 설치된 환경에서만 확인 가능하다. 개발지시서 §5 는 이 항목이 미충족이면 게시 단계로 넘어가지 말라고 규정한다.

- Experimental Instance 실행 테스트 (`F5`)
- Release VSIX 직접 설치 테스트 (`bin\Release\CopyCodeReference.vsix` 실행)
- 에디터 우클릭 메뉴에 두 명령이 실제로 표시되는지 확인. 이 머신에서는 VSCT 컴파일과 상수 대조까지만 검증했고, 메뉴가 화면에 뜨는 것은 확인하지 못했다.
- `Tools` → `Options` → `Copy Code Reference` → `General` 페이지가 열리고 네 항목 (위치 서식 라디오 3개, 경로 구분자 체크박스, 여러 줄 라디오 3개, 캐럿 줄 체크박스) 이 다 보이는지, 선택을 바꾼 뒤 `OK` 를 누르면 복사 결과가 바뀌는지, VS 재시작 후에도 유지되는지 확인. 이 머신에서는 pkgdef 의 `ToolsOptionsPages` 등록과 빌더 단위 테스트까지만 검증했다.
- 선택 영역 없이 명령을 실행했을 때 캐럿 줄이 복사되는지 확인 (옵션 on). 가상 공백만 덮은 선택에서도 같게 동작하는지 함께 확인. executor 경로라 이 머신에서 단위 테스트로 못 잡는다.
