import axios from 'axios'

export const sendRequest = async (request, data) => {
    try {
        const response = await axios.get(`http://localhost:8000/`, {
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
