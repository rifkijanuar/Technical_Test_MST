import React, { useState } from 'react'

function Register() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [email, setEmail] = useState('');
    const [fullname, setFullname] = useState('');
   function AddUser()
   {
    let items ={username,password,email,fullname}
    console.warn(items);
    fetch('https://localhost:7049/api/Users/register',
    {
        method:"POST",
        headers:
        {
            "Accept": "application/json",
            "Content-type": "application/json"
        },
        body:JSON.stringify(items)
    }).then((result)=>{
        result.json().then((resp)=>{
            console.warn(resp);
            alert(resp.statusMessage);
        })
    })
   }
    return (
        <body>

            <div className="container">
                <div className="card o-hidden border-0 shadow-lg my-5">
                    <div className="card-body p-0">
                        {/*  <!-- Nested Row within Card Body --> */}
                        <div className="row">
                            <div className="col-lg-5 d-none d-lg-block bg-register-image"></div>
                            <div className="col-lg-7">
                                <div className="p-5">
                                    <div className="text-center">
                                        <h1 className="h4 text-gray-900 mb-4">Create an Account!</h1>
                                    </div>
                                    <div className="user">
                                        <div className="form-group row">
                                            <input type="text" className="form-control form-control-user"
                                                value={username} onChange={(e) => { setUsername(e.target.value) }}
                                                placeholder="Username" />
                                        </div>
                                        <div className="form-group row">
                                            <input type="password" className="form-control form-control-user"
                                               value={password} onChange={(e) => { setPassword(e.target.value) }}
                                                placeholder="password" />
                                        </div>
                                        <div className="form-group row">
                                            <input type="text" className="form-control form-control-user"
                                               value={email} onChange={(e) => { setEmail(e.target.value) }}
                                                placeholder="Email" />
                                        </div>
                                        <div className="form-group row">
                                            <input type="text" className="form-control form-control-user"
                                              value={fullname} onChange={(e) => { setFullname(e.target.value) }}
                                                placeholder="Fullname" />
                                        </div>
                                        <button className="btn btn-primary btn-user btn-block" onClick={AddUser}>
                                            Register Account
                                        </button>
                                        <hr />
                                    </div>
                                    <hr />
                                    <div className="text-center">
                                        <a className="small" href="forgot-password">Forgot Password?</a>
                                    </div>
                                    <div className="text-center">
                                        <a className="small" href="/">Already have an account? Login!</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </body>
    )
}

export default Register
