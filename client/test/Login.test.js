// tests/Login.spec.js
import { mount } from '@vue/test-utils';
import { describe, it, expect } from 'vitest';
import Login from '@/views/Login.vue';

describe('Login.vue', () => {
  it('renders login form', () => {
    const wrapper = mount(Login);
    expect(wrapper.find('h2.login-title-txt').text()).toBe('Login');
  });

  it('show good error msg when empty fields', async () => {
    const wrapper = mount(Login);
    await wrapper.find('button.login-btn').trigger('click');
    expect(wrapper.find('.error-message').text()).toBe('Please fill all the fields');
  });

    it('show good error msg when empty password', async () => {
        const wrapper = mount(Login);
        await wrapper.setData({ email: 'example@app.com', password: '' });
        await wrapper.find('button.login-btn').trigger('click');
        expect(wrapper.find('.error-message').text()).toBe('Please fill all the fields');
    });

    it('show good error msg when empty email', async () => {
        const wrapper = mount(Login);
        await wrapper.setData({ email: '', password: 'password' });
        await wrapper.find('button.login-btn').trigger('click');
        expect(wrapper.find('.error-message').text()).toBe('Please fill all the fields');
    });

    it('show good error msg when invalid email', async () => {
        const wrapper = mount(Login);
        await wrapper.setData({ email: 'invalid-email', password: 'password' });
        await wrapper.find('button.login-btn').trigger('click');
        expect(wrapper.find('.error-message').text()).toBe('Please enter a valid email');
    });
});
