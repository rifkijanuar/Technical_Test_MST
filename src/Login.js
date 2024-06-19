import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom';

function Login() {
    const [username, setLoginName] = useState('');
    const [password, setPassword] = useState('');
    const navigate=useNavigate();
    function GetLoginDetails() {
        let items = { username, password }
        console.warn(items);
        fetch('https://localhost:7049/api/Users/login',
            {
                method: "POST",
                headers:
                {
                    "Accept": "application/json",
                    "Content-type": "application/json"
                },
                body: JSON.stringify(items)
            }).then((result) => {
                result.json().then((resp) => {
                    console.warn(resp);
                    navigate("/dashboard");
                })
            })
    }
    return (
        <div>

            <div className="container">

                {/*  <!-- Outer Row --> */}
                <div className="row justify-content-center">

                    <div className="col-xl-10 col-lg-12 col-md-9">

                        <div className="card o-hidden border-0 shadow-lg my-5">
                            <div className="card-body p-0">
                                {/* <!-- Nested Row within Card Body --> */}
                                <div className="row">
                                    <div className="col-lg-6 d-none d-lg-block bg-login-image"></div>
                                    <div className="col-lg-6">
                                        <div className="p-5">
                                            <div className="text-center">
                                                <h1 className="h4 text-gray-900 mb-4">Welcome!</h1>
                                            </div>
                                            <div className="user">
                                                <div className="form-group">
                                                    <input type="text" className="form-control form-control-user"
                                                        value={username} onChange={(e) => { setLoginName(e.target.value) }}
                                                        placeholder="Username" />
                                                </div>
                                                <div className="form-group">
                                                    <input type="password" className="form-control form-control-user"
                                                        value={password} onChange={(e) => { setPassword(e.target.value) }}
                                                        placeholder="Password" />
                                                </div>
                                                <button className="btn btn-primary btn-user btn-block" onClick={GetLoginDetails}>
                                                    Login
                                                </button>
                                                <hr />
                                            </div>
                                            <hr />
                                            <div className="text-center">
                                                <a className="small" href="forgot-password">Forgot Password?</a>
                                            </div>
                                            <div className="text-center">
                                                <a className="small" href="register">Create an Account!</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>

                </div>

            </div>

        </div>
    )
}

export default Login
