# CSsocketProgramming

Unity Client와 C#에서 지원하는 TCP socket Class를 통한 데이터 통신 기능 구현
Unity와 .NET은 JSON API를 통해 데이터를 주고 받고, .NET은 관련 데이터를 엔드 단에 있는 RDS(MySQL)에 CRUD 실행.

- 기술 스택
  
![image](https://github.com/Pyeonseohee/CSsocketProgramming/assets/58354506/34f98cfc-6c71-45c7-8669-9ebb61e5ddd2)



- API 목록
 1. 회원가입 API
  2. 로그인 API
  3. 오늘의 감정 GET API(오늘의 감정을 아바타에게 투영하기 위해)
  4. My Log GET API(사용자의 감정을 되돌아보기 위해)
  5. 채팅방 GET, POST API(사용자가 아바타와 채팅한 목록을 날짜 별로 생성)
  6. 채팅내용 GET, POST API(사용자가 날짜 별로 아바타와 어떤 대화를 했는지)
