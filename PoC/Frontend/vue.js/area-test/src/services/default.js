import axios from 'axios'

export const sendRequest = async (request, data) => {
    try {
        const response = await axios.get(`/`, {
            params: {
                request,
                data
            }
        });
        return response.data;
      } catch (error) {
        console.error('Error: ', error);
        throw error;
      }
};
