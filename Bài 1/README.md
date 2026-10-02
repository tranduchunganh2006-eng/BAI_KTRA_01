**CÂU 1: PHÂN BIỆT VALUE TYPES VÀ REFERENCE TYPES VỀ CƠ CHẾ LƯU TRỮ VÙNG NHỚ (STACK VS HEAP)**
Trong C#, các kiểu dữ liệu được chia thành hai nhóm chính: Value Types (Kiểu giá trị) và Reference Types (Kiểu tham chiếu). Hai kiểu này khác biệt cơ bản về cơ chế lưu trữ vùng nhớ và cách quản lý dữ liệu.

1. Phân tích chi tiết từng kiểu dữ liệu
a. Value Types (Kiểu giá trị)
Cơ chế lưu trữ vùng nhớ: Dữ liệu thực tế được lưu trữ trực tiếp tại địa chỉ vùng nhớ của biến. Đối với các biến cục bộ khai báo trong phương thức, dữ liệu được lưu trữ trên vùng nhớ Stack.
Cơ chế gán (Assignment): Khi gán một biến Value Type cho một biến khác, hệ thống sẽ thực hiện sao chép toàn bộ giá trị (Copy by value). Hai biến này hoàn toàn độc lập; thay đổi giá trị ở biến này không làm ảnh hưởng đến biến kia.
Quản lý bộ nhớ: Vùng nhớ trên Stack được quản lý tự động theo cơ chế LIFO (Last In, First Out). Dữ liệu sẽ tự động giải phóng ngay khi biến vượt ra khỏi phạm vi hoạt động (Scope).
Các kiểu dữ liệu đại diện: Các kiểu số nguyên/số thực (int, float, double...), kiểu đúng sai (bool), kiểu ký tự (char), struct, enum...
b. Reference Types (Kiểu tham chiếu)
Cơ chế lưu trữ vùng nhớ: Việc lưu trữ được chia làm 2 phần. Biến thực chất chỉ chứa một địa chỉ tham chiếu (con trỏ) lưu trên vùng nhớ Stack, còn đối tượng và dữ liệu thực tế lại nằm trên vùng nhớ Heap.
Cơ chế gán (Assignment): Khi gán một biến Reference Type cho một biến khác, hệ thống chỉ sao chép địa chỉ tham chiếu (Copy by reference). Cả hai biến lúc này đều trỏ chung về một đối tượng trên Heap. Thay đổi dữ liệu thông qua biến này sẽ làm thay đổi dữ liệu mà biến kia đang tham chiếu tới.
Quản lý bộ nhớ: Vùng nhớ Heap được quản lý bởi tiến trình dọn rác tự động Garbage Collector (GC). GC sẽ tự động phát hiện và thu hồi vùng nhớ khi không còn biến nào trỏ đến đối tượng đó nữa.
Các kiểu dữ liệu đại diện: Kiểu lớp (class), giao diện (interface), ủy nhiệm (delegate), chuỗi (string), đối tượng (object), mảng (array)...
2. Trường hợp đặc biệt về vị trí vùng nhớ
Nguyên tắc vị trí: Vùng nhớ của biến không chỉ phụ thuộc vào kiểu dữ liệu mà còn phụ thuộc vào nơi nó được khai báo.
Nếu một Value Type đóng vai trò là một thuộc tính hoặc trường dữ liệu (field) khai báo bên trong một Reference Type (ví dụ: một biến int nằm trong một class), thì giá trị của Value Type đó sẽ được lưu trên vùng nhớ Heap cùng với đối tượng chứa nó.

**CÂU 2: TÍNH NĂNG INIT-ONLY PROPERTIES (INIT) TRONG C# 9/10**
1. Sự khác biệt giữa init và set thông thường
Phương thức set (Thông thường): Cho phép thay đổi giá trị của thuộc tính ở bất kỳ thời điểm nào trong suốt vòng đời của đối tượng.
Phương thức init (Init-only): Chỉ cho phép gán giá trị tại thời điểm khởi tạo đối tượng (thông qua Hàm tạo - Constructor hoặc Object Initializer). Sau khi quá trình khởi tạo kết thúc, thuộc tính đó trở thành chỉ đọc (Read-only), không thể sửa đổi giá trị.
2. Trường hợp sử dụng thực tế
Tính năng init được sử dụng để xây dựng các Đối tượng bất biến (Immutable Objects).
Ứng dụng: Thường dùng trong các mô hình DTO (Data Transfer Object), cấu hình hệ thống (Configuration Settings), hoặc mô hình dữ liệu trong kiến trúc DDD (Domain-Driven Design).
Lợi ích: Đảm bảo tính toàn vẹn dữ liệu, tránh việc vô tình làm thay đổi trạng thái đối tượng ở các tầng xử lý khác, đồng thời giúp mã nguồn an toàn khi chạy trong môi trường đa luồng (Thread-safe).

**CÂU 3: PHÂN BIỆT PHƯƠNG THỨC VIRTUAL (LỚP CHA) VÀ OVERRIDE (LỚP CON) TRONG TÍNH ĐA HÌNH**
Trong lập trình hướng đối tượng (OOP) với C#, tính Đa hình (Polymorphism) cho phép đối tượng thuộc các lớp khác nhau phản ứng khác nhau trước cùng một lời gọi phương thức.
1. Bản chất của virtual và override
Phương thức virtual (Ở Lớp cha):
Cho phép phương thức có thể bị ghi đè bởi các lớp kế thừa.
Phải chứa phần thân mặc định ở lớp cha.
Phương thức override (Ở Lớp con):
Thể hiện việc ghi đè hoặc thay thế logic mặc định của lớp cha bằng logic riêng của lớp con.
Chỉ có thể dùng trên phương thức đã được đánh dấu là virtual, abstract, hoặc override ở lớp cha.
2. Cơ chế thực thi Runtime (Virtual Method Table - VMT)
Khi gọi một phương thức thông qua con trỏ kiểu lớp cha nhưng tham chiếu đến đối tượng của lớp con, C# sẽ kiểm tra bảng phương thức ảo (VMT) tại thời điểm chạy (Runtime) để gọi chính xác phiên bản override của lớp con thay vì phương thức virtual của lớp cha.

**CÂU 4: TẠI SAO THÀNH PHẦN STATIC KHÔNG THỂ TRUY XUẤT QUA THỂ HIỆN (OBJECT INSTANCE)?**
Một thành phần (biến, phương thức, thuộc tính) được khai báo với từ khóa static trong C# không thể truy xuất thông qua một đối tượng tạo bởi toán tử new vì những lý do sau:
1. Sự khác biệt về cấp độ sở hữu 
Thành phần thể hiện (Instance Member): Thuộc về từng đối tượng cụ thể. Mỗi đối tượng tạo ra bởi từ khóa new sẽ có một bản sao vùng nhớ riêng biệt cho các thành phần này.
Thành phần tĩnh (Static Member): Thuộc về cấp độ Lớp (Class level). Mọi thể hiện của lớp đều chia sẻ chung một thành phần static duy nhất.
2. Quản lý bộ nhớ
Thành phần static được trình thực thi (.NET Runtime) cấp phát bộ nhớ duy nhất một lần khi Lớp được nạp vào bộ nhớ lần đầu tiên, và nó tồn tại suốt vòng đời ứng dụng. Ngược lại, đối tượng (Instance) chỉ được tạo trên Heap khi gọi toán tử new. Do đó, thành phần static tồn tại độc lập, không phụ thuộc vào việc có đối tượng nào được tạo ra hay chưa.
3. Lý do thiết kế ngữ nghĩa
Để đảm bảo tính rõ ràng trong mã nguồn và tránh gây hiểu lầm cho lập trình viên:
Nếu C# cho phép viết instance.StaticMember, người đọc mã nguồn dễ lầm tưởng rằng StaticMember là thuộc tính riêng của thể hiện instance đó.
Trình biên dịch C# bắt buộc truy xuất thành phần tĩnh qua tên lớp (ClassName.StaticMember) nhằm khẳng định rõ ràng thành phần này là dùng chung cho toàn bộ Lớp.