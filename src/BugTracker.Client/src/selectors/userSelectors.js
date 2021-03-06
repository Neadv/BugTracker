const selectedUser = (state) => state.users.selectedUser;
const errors = (state) => state.users.errors;
const loading = (state) => state.users.loading;

const users = (isActive = true) => (
    state => isActive ? state.users.activeUsers : state.users.unactiveUsers
)

export const usersSelectors = {
    selectedUser,
    errors,
    users,
    loading
}
