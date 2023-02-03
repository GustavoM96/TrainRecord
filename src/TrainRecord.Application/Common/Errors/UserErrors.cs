using ErrorOr;

namespace TrainRecord.Application.Errors
{
    public static class UserError
    {
        public static Error EmailExists =>
            Error.Conflict("User.EmailExists", "email de usuário já cadastrado");
        public static Error UpdateInvalid =>
            Error.Conflict(
                "User.UpdateInvalid",
                "não permitido atualizar usuário com os seguintes campos(email, senha, id)"
            );
        public static Error NotFound =>
            Error.NotFound("User.EmailExists", "usuário não encontrado");

        public static Error LoginInvalid =>
            Error.NotFound("User.NotFound", "conta ou senha inválida");

        public static Error IsNotOwnerResourceAndAdm =>
            Error.Failure(
                "User.IsNotOwnerResource",
                "usuário não é dono do recurso ou não tem acesso de administrador"
            );
        public static Error RegisterAdmInvalid =>
            Error.Failure(
                "User.RegisterAdmInvalid",
                "usuário não pode registrar um adm, pois este não tem permissão de administrador"
            );
        public static Error IsNotAdm =>
            Error.Failure("User.IsNotAdm", "usuário não tem acesso de administrador");

        public static Error TeacherNotFound =>
            Error.NotFound("User.TeacherNotFound", "professor não encontrado");
        public static Error IsNotTeacher =>
            Error.Conflict(
                "User.IsNotTeacher",
                "professor a quem usuário deseja ser aluno, não tem regra de professor"
            );
        public static Error TeacherStudentExists =>
            Error.Conflict("User.TeacherStudentExists", "usuário já é aluno desse professor");

        public static Error TeacherStudentNotFound =>
            Error.NotFound(
                "User.TeacherStudentNotFound",
                "não encontrado vinculação entre usuário e professor"
            );
    }
}
