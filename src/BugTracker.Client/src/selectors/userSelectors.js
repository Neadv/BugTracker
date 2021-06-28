const selectedUser = (state) => state.users.selectedUser;
const errors = (state) => state.users.errors; 


export const usersSelectors = {
    selectedUser,
    errors
}
